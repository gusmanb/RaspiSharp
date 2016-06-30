using Newtonsoft.Json;
using RaspiSharp;
using RaspiSharp.Software;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RaspiImporter
{
    [Serializable]
	public class BaseElement
	{
        public BaseElement() { }
        [field: NonSerialized]
        public event EventHandler NameChanged;
		string name;
		[Category("Design")]
		[DisplayName("(Name)")]
		public string Name { get { return name; } set { name = value; if(NameChanged != null)NameChanged(this, EventArgs.Empty); } }

        string className;

		[Browsable(false)]
		public string InternalClassName { get { return className; } set { className = value; } }

        [Category("Design")]
        [DisplayName("(Class name)")]
        [Browsable(true)]
        [JsonIgnore]
        public string ClassName { get { return className; } }

        [Browsable(false)]
		[JsonIgnore]
		public Type ClassType { get; set; }

		[Browsable(false)]
		[JsonIgnore]
		public ElementInputType[] Inputs { get; set; }

		[Browsable(false)]
		[JsonIgnore]
		public ElementOutputType[] Outputs { get; set; }

		[Browsable(false)]
		[JsonIgnore]
		public ElementPropertyType[] Properties { get; set; }

		[Browsable(false)]
		[JsonIgnore]
		public BaseElementFunctionInfo[] Functions { get; set; }

	}

    [Serializable]
    public class ElementInputType
	{

		[Browsable(false)]
		public IOType InputType { get; set; }
		[Browsable(false)]
		public string InputName { get; set; }
		[Browsable(false)]
		public BaseElement Parent { get; set; }
	}

    [Serializable]
    public class ElementOutputType
	{

		[Browsable(false)]
		public IOType OutputType { get; set; }
		[Browsable(false)]
		public string OutputName { get; set; }
		[Browsable(false)]
		public BaseElement Parent { get; set; }
	}

    [Serializable]
    public class ElementPropertyType
	{
		[Browsable(false)]
		public string PropertyName { get; set; }
		[Browsable(false)]
		public string PropertyType { get; set; }
		[Browsable(false)]
		public BaseElement Parent { get; set; }
	}

    [Serializable]
    public class BaseElementFunctionInfo
	{
		public BaseElement Parent { get; set; }
		public string FunctionName { get; set; }
	
	}

    [Serializable]
    public class BaseElementLink
	{
		public ElementInputType Input { get; set; }
		public bool InputOnLeft { get; set; }
		public ElementOutputType Output { get; set; }
		public bool OutputOnLeft { get; set; }
	}

	public class ElementImporter
	{
		Dictionary<string, List<BaseElement>> categorizedElementTypes = new Dictionary<string, List<BaseElement>>();
		Dictionary<string, BaseElement> elemntsByType = new Dictionary<string, BaseElement>();

		public List<BaseElement> this[string Key]
		{

			get { return categorizedElementTypes[Key]; }
		
		}

		public IEnumerable<string> Keys {

			get { return categorizedElementTypes.Keys; }
		
		}

		public BaseElement[] Elements
		{

			get { return categorizedElementTypes.SelectMany(s => s.Value).ToArray(); }
		
		}

		public Dictionary<string, BaseElement> ElementsByType
		{

			get { return elemntsByType; }
		
		}

		public ElementImporter()
		{

			var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).Where(t => t.BaseType == typeof(RaspElement) || t.BaseType == typeof(RaspPort)).ToArray();

			AssemblyName asmName = new AssemblyName();
			asmName.Name = "RaspiElements";
			AssemblyBuilder asmBuild = Thread.GetDomain().DefineDynamicAssembly(asmName, AssemblyBuilderAccess.Run);
			
			ModuleBuilder modBuild = asmBuild.DefineDynamicModule("TypeModule");   

			ConstructorInfo infoCategory = typeof(CategoryAttribute).GetConstructor(new Type[]{ typeof(string) });
			PropertyInfo infoFieldCategory = typeof(CategoryAttribute).GetProperty("Category");

			foreach (var t in types)
			{

                if (t == typeof(RaspElement) || t == typeof(RaspPort))
                    continue;

				var attrs = t.GetCustomAttributes(true);

				string Category = "Unknown";

				foreach (var attr in attrs)
				{

					if (attr is RaspElementCategoryAttribute)
					{

						Category = (attr as RaspElementCategoryAttribute).Category;
						break;
					
					}
				
				}

				List<BaseElement> storedTypes;

				if (!categorizedElementTypes.ContainsKey(Category))
				{

					storedTypes = new List<BaseElement>();
					categorizedElementTypes.Add(Category, storedTypes);

				}
				else
					storedTypes = categorizedElementTypes[Category];

				    
				TypeBuilder tb = modBuild.DefineType(t.Name + "Info", TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.Serializable, typeof(BaseElement));

				List<ElementPropertyType> properts = new List<ElementPropertyType>();

				var properties = t.GetProperties();

				foreach (var prop in properties)
				{

					var attr = Attribute.GetCustomAttribute(prop, typeof(RaspPropertyAttribute), true);

					if (attr != null)
					{
						

						if (prop.PropertyType == typeof(RaspBuffer))
						{
							var propB = AddProperty(tb, typeof(string), prop.Name);

							propB.SetCustomAttribute(
								new CustomAttributeBuilder(
									typeof(TypeConverterAttribute).
									GetConstructor(new Type[] { typeof(Type) })
									, new object[] { typeof(BufferConverter) }));

						}
						else
						{
							var propB = AddProperty(tb, prop.PropertyType, prop.Name, prop.PropertyType == typeof(TaskMode) ? "Software" : "Hardware");

							if (prop.PropertyType == typeof(byte[]))
							{

								propB.SetCustomAttribute(
									new CustomAttributeBuilder(
										typeof(TypeConverterAttribute).
										GetConstructor(new Type[] { typeof(Type) })
										, new object[] { typeof(ByteArrayConverter) }));

							}
						}

						properts.Add(new ElementPropertyType { PropertyName = prop.Name, PropertyType = prop.PropertyType.Name });
					}
				}

				
				List<BaseElementFunctionInfo> functions = new List<BaseElementFunctionInfo>();
				List<ElementInputType> inputs = new List<ElementInputType>();
				List<ElementOutputType> outputs = new List<ElementOutputType>();

				var methods = t.GetMethods();

				foreach (var method in methods)
				{

					var attr = Attribute.GetCustomAttribute(method, typeof(RaspMethodAttribute), true);

					if (attr != null)
					{

						TypeBuilder tbm = modBuild.DefineType( t.Name + method.Name + "FunctionInfo" + method.GetHashCode(), TypeAttributes.Class | TypeAttributes.Serializable, typeof(BaseElementFunctionInfo));
						var pInfo = method.GetParameters();

						foreach (var para in pInfo)
							AddProperty(tbm, para.ParameterType, para.Name);
							


						var clas = tbm.CreateType();
						var mifo = (BaseElementFunctionInfo)Activator.CreateInstance(clas);
						mifo.FunctionName = method.Name;

						functions.Add(mifo);

					}

					attr = Attribute.GetCustomAttribute(method, typeof(RaspInputAttribute), true);

					if (attr != null)
					{

						var rAttr = (RaspInputAttribute)attr;
						inputs.Add(new ElementInputType { InputName = method.Name, InputType = rAttr.InputType });
					
					}
				
				}

				var events = t.GetEvents();

				foreach (var evt in events)
				{

					var attr = Attribute.GetCustomAttribute(evt, typeof(RaspOutputAttribute), true);

					if (attr != null)
					{

						var rAttr = (RaspOutputAttribute)attr;
						outputs.Add(new ElementOutputType { OutputName = evt.Name, OutputType = rAttr.OutputType });

					}
				
				}

				var tType = tb.CreateType();
				var elem = (BaseElement)Activator.CreateInstance(tType);
				elem.Functions = functions.ToArray();
				elem.Inputs = inputs.ToArray();
				elem.Outputs = outputs.ToArray();
				elem.Properties = properts.ToArray();
				elem.InternalClassName = t.Name;

				elem.ClassType = t;
				elem.Name = t.Name;

				storedTypes.Add(elem);

				elemntsByType.Add(elem.InternalClassName, elem);
			}

		}

		PropertyBuilder AddProperty(TypeBuilder typeBuilder, Type PropertyType, string PropertyName, string PropertyCategory = "Hardware")
		{

			FieldBuilder fieldBuilder = typeBuilder.DefineField(PropertyName + "_storage", PropertyType, FieldAttributes.Private);
			PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(PropertyName, PropertyAttributes.HasDefault, PropertyType, new Type[] { });
			MethodBuilder propertyGetter = typeBuilder.DefineMethod("get_" + PropertyName, MethodAttributes.Public | MethodAttributes.HideBySig, PropertyType, new Type[] {  });
			var ilGenerator = propertyGetter.GetILGenerator();
			ilGenerator.Emit(OpCodes.Ldarg_0);
			ilGenerator.Emit(OpCodes.Ldfld, fieldBuilder);
			ilGenerator.Emit(OpCodes.Ret);

			MethodBuilder propertySetter = typeBuilder.DefineMethod("set_" + PropertyName, MethodAttributes.Public | MethodAttributes.HideBySig, null, new Type[] { PropertyType });
			var propertySetterIl = propertySetter.GetILGenerator();
			propertySetterIl.Emit(OpCodes.Ldarg_0);
			propertySetterIl.Emit(OpCodes.Ldarg_1);
			propertySetterIl.Emit(OpCodes.Stfld, fieldBuilder);
			propertySetterIl.Emit(OpCodes.Ret);

			propertyBuilder.SetGetMethod(propertyGetter);
			propertyBuilder.SetSetMethod(propertySetter);

			propertyBuilder.SetCustomAttribute(
								new CustomAttributeBuilder(
									typeof(CategoryAttribute).
									GetConstructor(new Type[] { typeof(string) })
									, new object[] { PropertyCategory })); 

			propertyBuilder.SetCustomAttribute(
								new CustomAttributeBuilder(
									typeof(DesignerSerializationVisibilityAttribute).
									GetConstructor(new Type[] { typeof(DesignerSerializationVisibility) })
									, new object[] { DesignerSerializationVisibility.Content })); 


			return propertyBuilder;
		
		}

		public string SerializeDeviceFile(RaspiDeviceFile File)
		{

			JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
			return JsonConvert.SerializeObject(File, Formatting.Indented, settings);

		}

		public RaspiDeviceFile DeserializeDeviceFile(string FileContent)
		{
			var elems = Elements;

			JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All, Binder = new RaspBinder(elems) };
			var file = JsonConvert.DeserializeObject<RaspiDeviceFile>(FileContent, settings);

			foreach (var elem in file.Elements)
			{

				var instance = elems.Where(e => e.GetType().Name == elem.GetType().Name).FirstOrDefault();

				if (instance != null)
				{
					elem.ClassType = instance.ClassType;

					List<ElementInputType> inputs = new List<ElementInputType>();
					List<ElementOutputType> outputs = new List<ElementOutputType>();
					List<BaseElementFunctionInfo> functions = new List<BaseElementFunctionInfo>();
					List<ElementPropertyType> props = new List<ElementPropertyType>();

					foreach (var input in instance.Inputs)
					{
						inputs.Add(new ElementInputType { InputName = input.InputName, InputType = input.InputType, Parent = elem });
					}
					foreach (var output in instance.Outputs)
						outputs.Add(new ElementOutputType { OutputName = output.OutputName, OutputType = output.OutputType, Parent = elem });

					foreach (var func in instance.Functions)
					{
						var newFunc = (BaseElementFunctionInfo)Activator.CreateInstance(func.GetType());
						newFunc.Parent = elem;
						newFunc.FunctionName = func.FunctionName;
						functions.Add(newFunc);
					}

					foreach (var prop in instance.Properties)
						props.Add(new ElementPropertyType { PropertyName = prop.PropertyName, PropertyType = prop.PropertyType, Parent = elem });

					elem.Inputs = inputs.ToArray();
					elem.Outputs = outputs.ToArray();
					elem.Functions = functions.ToArray();
					elem.Properties = props.ToArray();
				}
			}

			return file;
		
		}
	}

	public class BufferConverter : TypeConverter
	{
		public static Dictionary<string, object> bufferList = new Dictionary<string, object>();
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			return new StandardValuesCollection(bufferList.Keys);
		}
		
	}

	public class ByteArrayConverter : TypeConverter
	{

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(string))
			{
				return true;
			}
			return base.CanConvertFrom(context, sourceType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			if (value is string)
			{

                var valString = (string)value;

                if (valString.StartsWith("@"))
                    return Encoding.ASCII.GetBytes(valString.Substring(1));
                else
                {

                    List<byte> buffer = new List<byte>();


                    string[] bytes = valString.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    foreach (var v in bytes)
                    {

                        var s = v.Trim();

                        if (s.Length != 4 || s.Substring(0, 2).ToLower() != "0x")
                            throw new InvalidCastException();

                        byte b;

                        if (!byte.TryParse(s.Substring(2), System.Globalization.NumberStyles.AllowHexSpecifier, null, out b))
                            throw new InvalidCastException();

                        buffer.Add(b);

                    }

                    return buffer.ToArray();
                }

			}
			return base.ConvertFrom(context, culture, value);
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(string))
			{
				return true;
			}
			return base.CanConvertTo(context, destinationType);
		}

		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string))
			{

				if (value == null)
					return null;

				byte[] data = (byte[])value;

				StringBuilder sb = new StringBuilder();

				foreach (byte b in data)
					sb.Append("0x" + b.ToString("X2") + ", ");

				sb.Remove(sb.Length - 2, 2);

				return sb.ToString();
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}

	public class RaspBinder : SerializationBinder
	{
		Type[] types;

		public RaspBinder(BaseElement[] elements)
		{
			List<Type> types = new List<Type>();

			foreach (var e in elements)
				types.Add(e.GetType());

			this.types = types.ToArray();
		}

		public override Type BindToType(string assemblyName, string typeName)
		{
			if (assemblyName == "RaspiElements")
			{

				var type = types.Where(t => t.Name == typeName).FirstOrDefault();

				if (type != null)
					return type;

			}
			return Type.GetType(typeName + ", " + assemblyName);
		}
	}

    [Serializable]
    public class RaspiDeviceFile
	{

		public BaseElement[] Elements { get; set; }
		public RaspPosition[] Positions { get; set; }
		public RaspLink[] Links { get; set; }
	}

    [Serializable]
    public class RaspPosition
	{

		public string ElementName { get; set; }
		public int X { get; set; }
		public int Y { get; set; }

	}

    [Serializable]
    public class RaspLink
	{

		public string InputDevice { get; set; }
		public string InputName { get; set; }
		public bool InputLinkOnLeft { get; set; }
		public string OutputDevice { get; set; }
		public string OutputName { get; set; }
		public bool OutputLinkOnLeft { get; set; }

	}
}
