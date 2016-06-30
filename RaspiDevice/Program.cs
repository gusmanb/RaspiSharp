using RaspiImporter;
using RaspiSharp;
using RaspiSharp.Software;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static BCM2835.BCM2835Managed;
using BCM2835;

namespace RaspiDevice
{
	class Program
	{
		static ElementImporter importer = new ElementImporter();
		static void Main(string[] args)
		{
			BCM2835Managed.bcm2835_init();

			RaspiDeviceFile file = importer.DeserializeDeviceFile(File.ReadAllText("counter.rd"));//args[0]));

			Dictionary<string, RaspElement> generalElements = new Dictionary<string, RaspElement>();
			//List<RaspElement> fixedSignals = new List<RaspElement>();

			Console.WriteLine("\nCreating elements...");

			//Primera fase, creamos las instancias y asignamos las propiedades
			foreach (var element in file.Elements)
			{
				var instance = (RaspElement)Activator.CreateInstance(element.ClassType);

				generalElements.Add(element.Name, instance);

				Console.WriteLine("\nAdding element \"" + element.Name + "\" of type " + element.ClassType.Name); 

				var srcProps = element.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.Public);
				var dstProps = instance.GetType().GetProperties();

				foreach (var srcProp in srcProps)
				{

					var dstProp = dstProps.Where(p => p.Name == srcProp.Name && p.PropertyType == srcProp.PropertyType).FirstOrDefault();

					if (dstProp != null)
					{
						var value = srcProp.GetValue(element);

						Console.WriteLine("Setting property " + dstProp.Name + " with value " + StringValue(value));
						dstProp.SetValue(instance, value);
					}

					//Si no coincide el tipo de propiedad es porque és una asignación de otro objeto, se realiza en la segunda fase
				
				}
			}

			Console.WriteLine("\nBinding elements...\n");

			//Segunda fase, asignamos las propiedades que son objetos
			foreach (var element in file.Elements)
			{

				var currentInstance = generalElements[element.Name];

				var srcProps = element.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.Public);
				var dstProps = currentInstance.GetType().GetProperties();

				foreach (var srcProp in srcProps)
				{
					//La propiedad de origen ha de ser string
					if (srcProp.PropertyType != typeof(string))
						continue;

					var dstProp = dstProps.Where(p => p.Name == srcProp.Name).FirstOrDefault();

					//Y la de destino no debe ser string
					if (dstProp != null && dstProp.PropertyType != typeof(string))
					{

						var objName = (string)srcProp.GetValue(element);
						if (!string.IsNullOrWhiteSpace(objName))
						{
							var instanceToSet = generalElements[objName];
							
							Console.WriteLine("Binding " + element.Name + "'s property " + dstProp.Name + " to " + objName);
							dstProp.SetValue(currentInstance, instanceToSet);
						}
					}

				}

			}

			Console.WriteLine("\nLinking inputs/outputs...\n");

			//Tercera fase, asignamos las entradas/salidas
			foreach (var link in file.Links)
			{

				var inDevice = generalElements[link.InputDevice];
				var outDevice = generalElements[link.OutputDevice];

				var inType = inDevice.GetType();
				var outType = outDevice.GetType();

				var inMethod = inType.GetMethod(link.InputName);
				var outEvent = outType.GetEvent(link.OutputName);

				Console.WriteLine("Linking " + link.InputDevice +"'s input \"" + link.InputName + "\" to " + link.OutputDevice + "'s output \""+ link.OutputName + "\"");

				outEvent.AddEventHandler(outDevice, inMethod.CreateDelegate(outEvent.EventHandlerType, inDevice));

			}

			Console.WriteLine("\nInitializing fixzed signals...\n");

			//Cuarta fase, arrancamos las señales fijas
			foreach (var element in file.Elements)
			{
				if (element.ClassType == typeof(RaspFixedByte) || element.ClassType == typeof(RaspFixedSignal))
				{
					var signal = generalElements[element.Name];
					var init = signal.GetType().GetMethod("Init");
					Console.WriteLine("Initializing signal " + element.Name);
					init.Invoke(signal, null);
				}
			}

			Console.WriteLine("\n\n<--Device ready, press CTRL+C to end execution-->");

			while (true)
				Console.ReadKey();

			BCM2835Managed.bcm2835_close();

		}

		static string StringValue(object Value)
		{

			return Value == null ? "(NULL)" : Value.ToString();
		
		}
	}
}
