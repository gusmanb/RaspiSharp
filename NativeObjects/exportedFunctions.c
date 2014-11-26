#include "exportedFunctions.h"


eventData eventDescriptors[64];


uint32_t bcm2835_gpio_full_eds()
{
	volatile uint32_t* paddr = bcm2835_gpio + BCM2835_GPEDS0 / 4;
	return bcm2835_peri_read(paddr);
}

void bcm2835_gpio_full_set_eds(uint8_t pinMask)
{
	volatile uint32_t* paddr = bcm2835_gpio + BCM2835_GPEDS0 / 4;
	bcm2835_peri_write(paddr, pinMask);
}



uint8_t readBitBangByte(uint8_t pin, uint8_t clockPin, uint8_t polarity, uint32_t lowDelay, uint32_t highDelay)
{


	bcm2835_gpio_fsel(pin, BCM2835_GPIO_FSEL_INPT);
	bcm2835_gpio_fsel(clockPin, BCM2835_GPIO_FSEL_OUTP);

	uint8_t value = 0;
	uint8_t c = 0;
	if (polarity)
	{
		for (c = 0; c < 8; c++)
		{
			bcm2835_gpio_clr(clockPin);
			bcm2835_delayMicroseconds(lowDelay);
			bcm2835_gpio_set(clockPin);
			bcm2835_delayMicroseconds(highDelay);
			value = value | (bcm2835_gpio_lev(pin) << c);
		}
	}
	else
	{
		for (c = 0; c < 8; c++)
		{
			bcm2835_gpio_set(clockPin);
			bcm2835_delayMicroseconds(lowDelay);
			bcm2835_gpio_clr(clockPin);
			bcm2835_delayMicroseconds(highDelay);
			value = value | (bcm2835_gpio_lev(pin) << c);
		}
	}

	return value;
}

void readBitBangBuffer(uint8_t* buffer, uint32_t length, uint8_t pin, uint8_t clockPin, uint8_t polarity, uint32_t lowDelay, uint32_t highDelay)
{


	bcm2835_gpio_fsel(pin, BCM2835_GPIO_FSEL_INPT);
	bcm2835_gpio_fsel(clockPin, BCM2835_GPIO_FSEL_OUTP);

	uint8_t value = 0;
	uint32_t x = 0;
	uint8_t c = 0;

	if (polarity)
	{
		for (x = 0; x < length; x++)
		{
			for (c = 0; c < 8; c++)
			{
				bcm2835_gpio_clr(clockPin);
				bcm2835_delayMicroseconds(lowDelay);
				bcm2835_gpio_set(clockPin);
				bcm2835_delayMicroseconds(highDelay);
				value = value | (bcm2835_gpio_lev(pin) << c);
			}

			buffer[x] = value;
			value = 0;

		}
	}
	else
	{
		for (x = 0; x < length; x++)
		{
			for (c = 0; c < 8; c++)
			{
				bcm2835_gpio_set(clockPin);
				bcm2835_delayMicroseconds(lowDelay);
				bcm2835_gpio_clr(clockPin);
				bcm2835_delayMicroseconds(highDelay);
				value = value | (bcm2835_gpio_lev(pin) << c);
			}

			buffer[x] = value;
			value = 0;
		}
	}
}

void writeBitBangByte(uint8_t data, uint8_t pin, uint8_t clockPin, uint8_t polarity, uint32_t lowDelay, uint32_t highDelay)
{

	bcm2835_gpio_fsel(pin, BCM2835_GPIO_FSEL_OUTP);
	bcm2835_gpio_fsel(clockPin, BCM2835_GPIO_FSEL_OUTP);

	uint8_t c = 0;

	if (polarity)
	{
		for (c = 0; c < 8; c++)
		{
			bcm2835_gpio_clr(clockPin);
			bcm2835_delayMicroseconds(lowDelay);

			if(data & (1 << c))
				bcm2835_gpio_set(pin);
			else
				bcm2835_gpio_clr(pin);

			bcm2835_gpio_set(clockPin);
			bcm2835_delayMicroseconds(highDelay);
		}
	}
	else
	{
		for (c = 0; c < 8; c++)
		{
			bcm2835_gpio_set(clockPin);
			bcm2835_delayMicroseconds(lowDelay);

			if (data & (1 << c))
				bcm2835_gpio_set(pin);
			else
				bcm2835_gpio_clr(pin);

			bcm2835_gpio_clr(clockPin);
			bcm2835_delayMicroseconds(highDelay);
		}
	}

}

void writeBitBangBuffer(uint8_t* buffer, uint32_t length, uint8_t pin, uint8_t clockPin, uint8_t polarity, uint32_t lowDelay, uint32_t highDelay)
{

	bcm2835_gpio_fsel(pin, BCM2835_GPIO_FSEL_OUTP);
	bcm2835_gpio_fsel(clockPin, BCM2835_GPIO_FSEL_OUTP);

	uint8_t data = 0;
	uint32_t x = 0;
	uint8_t c = 0;

	if (polarity)
	{
		for (x = 0; x < length; x++)
		{
			data = buffer[x];

			for (c = 0; c < 8; c++)
			{
				bcm2835_gpio_clr(clockPin);
				bcm2835_delayMicroseconds(lowDelay);

				if (data & (1 << c))
					bcm2835_gpio_set(pin);
				else
					bcm2835_gpio_clr(pin);

				bcm2835_gpio_set(clockPin);
				bcm2835_delayMicroseconds(highDelay);
			}
		}
	}
	else
	{
		for (x = 0; x < length; x++)
		{
			data = buffer[x];

			for (c = 0; c < 8; c++)
			{
				bcm2835_gpio_set(clockPin);
				bcm2835_delayMicroseconds(lowDelay);

				if (data & (1 << c))
					bcm2835_gpio_set(pin);
				else
					bcm2835_gpio_clr(pin);

				bcm2835_gpio_clr(clockPin);
				bcm2835_delayMicroseconds(highDelay);
			}
		}
	}

}



uint8_t readNibbleByte(uint8_t pin0, uint8_t pin1, uint8_t pin2, uint8_t pin3, uint8_t clockPin, uint8_t polarity, uint32_t lowDelay, uint32_t highDelay)
{

	bcm2835_gpio_fsel(pin0, BCM2835_GPIO_FSEL_INPT);
	bcm2835_gpio_fsel(pin1, BCM2835_GPIO_FSEL_INPT);
	bcm2835_gpio_fsel(pin2, BCM2835_GPIO_FSEL_INPT);
	bcm2835_gpio_fsel(pin3, BCM2835_GPIO_FSEL_INPT);
	bcm2835_gpio_fsel(clockPin, BCM2835_GPIO_FSEL_OUTP);

	uint8_t value = 0;

	if (polarity)
	{
			bcm2835_gpio_clr(clockPin);
			bcm2835_delayMicroseconds(lowDelay);
			bcm2835_gpio_set(clockPin);
			bcm2835_delayMicroseconds(highDelay);

			value = bcm2835_gpio_lev(pin0) | (bcm2835_gpio_lev(pin1) << 1) | (bcm2835_gpio_lev(pin2) << 2) | (bcm2835_gpio_lev(pin3) << 3);

			bcm2835_gpio_clr(clockPin);
			bcm2835_delayMicroseconds(lowDelay);
			bcm2835_gpio_set(clockPin);
			bcm2835_delayMicroseconds(highDelay);

			value |= (bcm2835_gpio_lev(pin0) << 4) | (bcm2835_gpio_lev(pin1) << 5) | (bcm2835_gpio_lev(pin2) << 6) | (bcm2835_gpio_lev(pin3) << 7);

	}
	else
	{
		bcm2835_gpio_set(clockPin);
		bcm2835_delayMicroseconds(lowDelay);
		bcm2835_gpio_clr(clockPin);
		bcm2835_delayMicroseconds(highDelay);

		value = bcm2835_gpio_lev(pin0) | (bcm2835_gpio_lev(pin1) << 1) | (bcm2835_gpio_lev(pin2) << 2) | (bcm2835_gpio_lev(pin3) << 3);

		bcm2835_gpio_set(clockPin);
		bcm2835_delayMicroseconds(lowDelay);
		bcm2835_gpio_clr(clockPin);
		bcm2835_delayMicroseconds(highDelay);

		value |= (bcm2835_gpio_lev(pin0) << 4) | (bcm2835_gpio_lev(pin1) << 5) | (bcm2835_gpio_lev(pin2) << 6) | (bcm2835_gpio_lev(pin3) << 7);

	}

	return value;

}

void readNibbleBuffer(uint8_t* buffer, uint32_t length, uint8_t pin0, uint8_t pin1, uint8_t pin2, uint8_t pin3, uint8_t clockPin, uint8_t polarity, uint32_t lowDelay, uint32_t highDelay)
{

	bcm2835_gpio_fsel(pin0, BCM2835_GPIO_FSEL_INPT);
	bcm2835_gpio_fsel(pin1, BCM2835_GPIO_FSEL_INPT);
	bcm2835_gpio_fsel(pin2, BCM2835_GPIO_FSEL_INPT);
	bcm2835_gpio_fsel(pin3, BCM2835_GPIO_FSEL_INPT);
	bcm2835_gpio_fsel(clockPin, BCM2835_GPIO_FSEL_OUTP);

	uint8_t value = 0;
	uint32_t x = 0;

	if (polarity)
	{
		for (x = 0; x < length; x++)
		{
			bcm2835_gpio_clr(clockPin);
			bcm2835_delayMicroseconds(lowDelay);
			bcm2835_gpio_set(clockPin);
			bcm2835_delayMicroseconds(highDelay);

			value = bcm2835_gpio_lev(pin0) | (bcm2835_gpio_lev(pin1) << 1) | (bcm2835_gpio_lev(pin2) << 2) | (bcm2835_gpio_lev(pin3) << 3);

			bcm2835_gpio_clr(clockPin);
			bcm2835_delayMicroseconds(lowDelay);
			bcm2835_gpio_set(clockPin);
			bcm2835_delayMicroseconds(highDelay);

			value |= (bcm2835_gpio_lev(pin0) << 4) | (bcm2835_gpio_lev(pin1) << 5) | (bcm2835_gpio_lev(pin2) << 6) | (bcm2835_gpio_lev(pin3) << 7);

			buffer[x] = value;
			value = 0;
		}
	}
	else
	{
		for (x = 0; x < length; x++)
		{
			bcm2835_gpio_set(clockPin);
			bcm2835_delayMicroseconds(lowDelay);
			bcm2835_gpio_clr(clockPin);
			bcm2835_delayMicroseconds(highDelay);

			value = bcm2835_gpio_lev(pin0) | (bcm2835_gpio_lev(pin1) << 1) | (bcm2835_gpio_lev(pin2) << 2) | (bcm2835_gpio_lev(pin3) << 3);

			bcm2835_gpio_set(clockPin);
			bcm2835_delayMicroseconds(lowDelay);
			bcm2835_gpio_clr(clockPin);
			bcm2835_delayMicroseconds(highDelay);

			value |= (bcm2835_gpio_lev(pin0) << 4) | (bcm2835_gpio_lev(pin1) << 5) | (bcm2835_gpio_lev(pin2) << 6) | (bcm2835_gpio_lev(pin3) << 7);

			buffer[x] = value;
			value = 0;
		}

	}

}

void writeNibbleByte(uint8_t data, uint8_t pin0, uint8_t pin1, uint8_t pin2, uint8_t pin3, uint8_t clockPin, uint8_t polarity, uint32_t lowDelay, uint32_t highDelay)
{
	bcm2835_gpio_fsel(pin0, BCM2835_GPIO_FSEL_OUTP);
	bcm2835_gpio_fsel(pin1, BCM2835_GPIO_FSEL_OUTP);
	bcm2835_gpio_fsel(pin2, BCM2835_GPIO_FSEL_OUTP);
	bcm2835_gpio_fsel(pin3, BCM2835_GPIO_FSEL_OUTP);
	bcm2835_gpio_fsel(clockPin, BCM2835_GPIO_FSEL_OUTP);

	uint32_t lowNibbleSet = (((data & 1) ? 1 : 0) << pin0) |
		(((data & 2) ? 1 : 0) << pin1) |
		(((data & 4) ? 1 : 0) << pin2) |
		(((data & 8) ? 1 : 0) << pin3);

	uint32_t lowNibbleClear = (((data & 1) ? 0 : 1) << pin0) |
		(((data & 2) ? 0 : 1) << pin1) |
		(((data & 4) ? 0 : 1) << pin2) |
		(((data & 8) ? 0 : 1) << pin3);

	uint32_t highNibbleSet = (((data & 16) ? 1 : 0) << pin0) |
		(((data & 32) ? 1 : 0) << pin1) |
		(((data & 64) ? 1 : 0) << pin2) |
		(((data & 128) ? 1 : 0) << pin3);

	uint32_t highNibbleClear = (((data & 16) ? 0 : 1) << pin0) |
		(((data & 32) ? 0 : 1) << pin1) |
		(((data & 64) ? 0 : 1) << pin2) |
		(((data & 128) ? 0 : 1) << pin3);

	if (polarity)
	{
		bcm2835_gpio_clr(clockPin);
		bcm2835_delayMicroseconds(lowDelay);

		bcm2835_gpio_write_multi(lowNibbleSet, 1);
		bcm2835_gpio_write_multi(lowNibbleClear, 0);

		bcm2835_gpio_set(clockPin);
		bcm2835_delayMicroseconds(highDelay);

		bcm2835_gpio_clr(clockPin);
		bcm2835_delayMicroseconds(lowDelay);

		bcm2835_gpio_write_multi(highNibbleSet, 1);
		bcm2835_gpio_write_multi(highNibbleClear, 0);

		bcm2835_gpio_set(clockPin);
		bcm2835_delayMicroseconds(highDelay);

	}
	else
	{
		bcm2835_gpio_set(clockPin);
		bcm2835_delayMicroseconds(lowDelay);

		bcm2835_gpio_write_multi(lowNibbleSet, 1);
		bcm2835_gpio_write_multi(lowNibbleClear, 0);

		bcm2835_gpio_clr(clockPin);
		bcm2835_delayMicroseconds(highDelay);

		bcm2835_gpio_set(clockPin);
		bcm2835_delayMicroseconds(lowDelay);

		bcm2835_gpio_write_multi(highNibbleSet, 1);
		bcm2835_gpio_write_multi(highNibbleClear, 0);

		bcm2835_gpio_clr(clockPin);
		bcm2835_delayMicroseconds(highDelay);

	}

}

void writeNibbleBuffer(uint8_t* buffer, uint32_t length, uint8_t pin0, uint8_t pin1, uint8_t pin2, uint8_t pin3, uint8_t clockPin, uint8_t polarity, uint32_t lowDelay, uint32_t highDelay)
{
	bcm2835_gpio_fsel(pin0, BCM2835_GPIO_FSEL_OUTP);
	bcm2835_gpio_fsel(pin1, BCM2835_GPIO_FSEL_OUTP);
	bcm2835_gpio_fsel(pin2, BCM2835_GPIO_FSEL_OUTP);
	bcm2835_gpio_fsel(pin3, BCM2835_GPIO_FSEL_OUTP);
	bcm2835_gpio_fsel(clockPin, BCM2835_GPIO_FSEL_OUTP);

	uint32_t lowNibbleSet;
	uint32_t lowNibbleClear;
	uint32_t highNibbleSet;
	uint32_t highNibbleClear;

	uint32_t x;
	uint8_t data;

	if (polarity)
	{
		for (x = 0; x < length; x++)
		{
			data = buffer[x];

			lowNibbleSet = (((data & 1) ? 1 : 0) << pin0) |
				(((data & 2) ? 1 : 0) << pin1) |
				(((data & 4) ? 1 : 0) << pin2) |
				(((data & 8) ? 1 : 0) << pin3);

			lowNibbleClear = (((data & 1) ? 0 : 1) << pin0) |
				(((data & 2) ? 0 : 1) << pin1) |
				(((data & 4) ? 0 : 1) << pin2) |
				(((data & 8) ? 0 : 1) << pin3);

			highNibbleSet = (((data & 16) ? 1 : 0) << pin0) |
				(((data & 32) ? 1 : 0) << pin1) |
				(((data & 64) ? 1 : 0) << pin2) |
				(((data & 128) ? 1 : 0) << pin3);

			highNibbleClear = (((data & 16) ? 0 : 1) << pin0) |
				(((data & 32) ? 0 : 1) << pin1) |
				(((data & 64) ? 0 : 1) << pin2) |
				(((data & 128) ? 0 : 1) << pin3);

			bcm2835_gpio_clr(clockPin);
			bcm2835_delayMicroseconds(lowDelay);

			bcm2835_gpio_write_multi(lowNibbleSet, 1);
			bcm2835_gpio_write_multi(lowNibbleClear, 0);

			bcm2835_gpio_set(clockPin);
			bcm2835_delayMicroseconds(highDelay);

			bcm2835_gpio_clr(clockPin);
			bcm2835_delayMicroseconds(lowDelay);

			bcm2835_gpio_write_multi(highNibbleSet, 1);
			bcm2835_gpio_write_multi(highNibbleClear, 0);

			bcm2835_gpio_set(clockPin);
			bcm2835_delayMicroseconds(highDelay);
		}
	}
	else
	{
		for (x = 0; x < length; x++)
		{
			data = buffer[x];

			lowNibbleSet = (((data & 1) ? 1 : 0) << pin0) |
				(((data & 2) ? 1 : 0) << pin1) |
				(((data & 4) ? 1 : 0) << pin2) |
				(((data & 8) ? 1 : 0) << pin3);

			lowNibbleClear = (((data & 1) ? 0 : 1) << pin0) |
				(((data & 2) ? 0 : 1) << pin1) |
				(((data & 4) ? 0 : 1) << pin2) |
				(((data & 8) ? 0 : 1) << pin3);

			highNibbleSet = (((data & 16) ? 1 : 0) << pin0) |
				(((data & 32) ? 1 : 0) << pin1) |
				(((data & 64) ? 1 : 0) << pin2) |
				(((data & 128) ? 1 : 0) << pin3);

			highNibbleClear = (((data & 16) ? 0 : 1) << pin0) |
				(((data & 32) ? 0 : 1) << pin1) |
				(((data & 64) ? 0 : 1) << pin2) |
				(((data & 128) ? 0 : 1) << pin3);
			
			bcm2835_gpio_set(clockPin);
			bcm2835_delayMicroseconds(lowDelay);

			bcm2835_gpio_write_multi(lowNibbleSet, 1);
			bcm2835_gpio_write_multi(lowNibbleClear, 0);

			bcm2835_gpio_clr(clockPin);
			bcm2835_delayMicroseconds(highDelay);

			bcm2835_gpio_set(clockPin);
			bcm2835_delayMicroseconds(lowDelay);

			bcm2835_gpio_write_multi(highNibbleSet, 1);
			bcm2835_gpio_write_multi(highNibbleClear, 0);

			bcm2835_gpio_clr(clockPin);
			bcm2835_delayMicroseconds(highDelay);
		}

	}

}



uint8_t readOctetByte(uint8_t pinList[8], uint8_t clockPin, uint8_t polarity, uint32_t lowDelay, uint32_t highDelay)
{

	uint32_t x;

	for(x = 0; x < 8; x++)
		bcm2835_gpio_fsel(pinList[x], BCM2835_GPIO_FSEL_INPT);

	bcm2835_gpio_fsel(clockPin, BCM2835_GPIO_FSEL_OUTP);

	uint8_t value = 0;

	if (polarity)
	{
		bcm2835_gpio_clr(clockPin);
		bcm2835_delayMicroseconds(lowDelay);
		bcm2835_gpio_set(clockPin);
		bcm2835_delayMicroseconds(highDelay);

		value = bcm2835_gpio_lev(pinList[0]) | (bcm2835_gpio_lev(pinList[1]) << 1) | (bcm2835_gpio_lev(pinList[2]) << 2) | (bcm2835_gpio_lev(pinList[3]) << 3) |
			(bcm2835_gpio_lev(pinList[4]) << 4) | (bcm2835_gpio_lev(pinList[5]) << 5) | (bcm2835_gpio_lev(pinList[6]) << 6) | (bcm2835_gpio_lev(pinList[7]) << 7);
	}
	else
	{
		bcm2835_gpio_set(clockPin);
		bcm2835_delayMicroseconds(lowDelay);
		bcm2835_gpio_clr(clockPin);
		bcm2835_delayMicroseconds(highDelay);

		value = bcm2835_gpio_lev(pinList[0]) | (bcm2835_gpio_lev(pinList[1]) << 1) | (bcm2835_gpio_lev(pinList[2]) << 2) | (bcm2835_gpio_lev(pinList[3]) << 3) |
			(bcm2835_gpio_lev(pinList[4]) << 4) | (bcm2835_gpio_lev(pinList[5]) << 5) | (bcm2835_gpio_lev(pinList[6]) << 6) | (bcm2835_gpio_lev(pinList[7]) << 7);

	}

	return value;

}

void readOctetBuffer(uint8_t* buffer, uint32_t length, uint8_t pinList[8], uint8_t clockPin, uint8_t polarity, uint32_t lowDelay, uint32_t highDelay)
{
	uint32_t x;

	for (x = 0; x < 8; x++)
		bcm2835_gpio_fsel(pinList[x], BCM2835_GPIO_FSEL_INPT);

	bcm2835_gpio_fsel(clockPin, BCM2835_GPIO_FSEL_OUTP);
	
	if (polarity)
	{
		for (x = 0; x < length; x++)
		{
			bcm2835_gpio_clr(clockPin);
			bcm2835_delayMicroseconds(lowDelay);
			bcm2835_gpio_set(clockPin);
			bcm2835_delayMicroseconds(highDelay);

			buffer[x] = bcm2835_gpio_lev(pinList[0]) | (bcm2835_gpio_lev(pinList[1]) << 1) | (bcm2835_gpio_lev(pinList[2]) << 2) | (bcm2835_gpio_lev(pinList[3]) << 3) |
				(bcm2835_gpio_lev(pinList[4]) << 4) | (bcm2835_gpio_lev(pinList[5]) << 5) | (bcm2835_gpio_lev(pinList[6]) << 6) | (bcm2835_gpio_lev(pinList[7]) << 7);

		}
	}
	else
	{
		for (x = 0; x < length; x++)
		{
			bcm2835_gpio_set(clockPin);
			bcm2835_delayMicroseconds(lowDelay);
			bcm2835_gpio_clr(clockPin);
			bcm2835_delayMicroseconds(highDelay);

			buffer[x] = bcm2835_gpio_lev(pinList[0]) | (bcm2835_gpio_lev(pinList[1]) << 1) | (bcm2835_gpio_lev(pinList[2]) << 2) | (bcm2835_gpio_lev(pinList[3]) << 3) |
				(bcm2835_gpio_lev(pinList[4]) << 4) | (bcm2835_gpio_lev(pinList[5]) << 5) | (bcm2835_gpio_lev(pinList[6]) << 6) | (bcm2835_gpio_lev(pinList[7]) << 7);
		}
	}

}

void writeOctetByte(uint8_t data, uint8_t pinList[8], uint8_t clockPin, uint8_t polarity, uint32_t lowDelay, uint32_t highDelay)
{
	uint32_t x;

	for (x = 0; x < 8; x++)
		bcm2835_gpio_fsel(pinList[x], BCM2835_GPIO_FSEL_OUTP);

	bcm2835_gpio_fsel(clockPin, BCM2835_GPIO_FSEL_OUTP);

	uint32_t octetSet = (((data & 1) ? 1 : 0) << pinList[0]) |
		(((data & 2) ? 1 : 0) << pinList[1]) |
		(((data & 4) ? 1 : 0) << pinList[2]) |
		(((data & 8) ? 1 : 0) << pinList[3]) |
		(((data & 16) ? 1 : 0) << pinList[4]) |
		(((data & 32) ? 1 : 0) << pinList[5]) |
		(((data & 64) ? 1 : 0) << pinList[6]) |
		(((data & 128) ? 1 : 0) << pinList[7]);

	uint32_t octetClear = (((data & 1) ? 0 : 1) << pinList[0]) |
		(((data & 2) ? 0 : 1) << pinList[1]) |
		(((data & 4) ? 0 : 1) << pinList[2]) |
		(((data & 8) ? 0 : 1) << pinList[3]) |
		(((data & 16) ? 0 : 1) << pinList[4]) |
		(((data & 32) ? 0 : 1) << pinList[5]) |
		(((data & 64) ? 0 : 1) << pinList[6]) |
		(((data & 128) ? 0 : 1) << pinList[7]);

	if (polarity)
	{
		bcm2835_gpio_clr(clockPin);
		bcm2835_delayMicroseconds(lowDelay);

		bcm2835_gpio_write_multi(octetSet, 1);
		bcm2835_gpio_write_multi(octetClear, 0);

		bcm2835_gpio_set(clockPin);
		bcm2835_delayMicroseconds(highDelay);
	}
	else
	{
		bcm2835_gpio_set(clockPin);
		bcm2835_delayMicroseconds(lowDelay);

		bcm2835_gpio_write_multi(octetSet, 1);
		bcm2835_gpio_write_multi(octetClear, 0);

		bcm2835_gpio_clr(clockPin);
		bcm2835_delayMicroseconds(highDelay);

	}

}

void writeOctetBuffer(uint8_t* buffer, uint32_t length, uint8_t pinList[8], uint8_t clockPin, uint8_t polarity, uint32_t lowDelay, uint32_t highDelay)
{
	uint32_t x;

	for (x = 0; x < 8; x++)
		bcm2835_gpio_fsel(pinList[x], BCM2835_GPIO_FSEL_OUTP);

	bcm2835_gpio_fsel(clockPin, BCM2835_GPIO_FSEL_OUTP);

	uint32_t octetSet;
	uint32_t octetClear;

	uint8_t data;

	if (polarity)
	{
		for (x = 0; x < length; x++)
		{
			data = buffer[x];

			octetSet = (((data & 1) ? 1 : 0) << pinList[0]) |
				(((data & 2) ? 1 : 0) << pinList[1]) |
				(((data & 4) ? 1 : 0) << pinList[2]) |
				(((data & 8) ? 1 : 0) << pinList[3]) |
				(((data & 16) ? 1 : 0) << pinList[4]) |
				(((data & 32) ? 1 : 0) << pinList[5]) |
				(((data & 64) ? 1 : 0) << pinList[6]) |
				(((data & 128) ? 1 : 0) << pinList[7]);

			octetClear = (((data & 1) ? 0 : 1) << pinList[0]) |
				(((data & 2) ? 0 : 1) << pinList[1]) |
				(((data & 4) ? 0 : 1) << pinList[2]) |
				(((data & 8) ? 0 : 1) << pinList[3]) |
				(((data & 16) ? 0 : 1) << pinList[4]) |
				(((data & 32) ? 0 : 1) << pinList[5]) |
				(((data & 64) ? 0 : 1) << pinList[6]) |
				(((data & 128) ? 0 : 1) << pinList[7]);

			bcm2835_gpio_clr(clockPin);
			bcm2835_delayMicroseconds(lowDelay);

			bcm2835_gpio_write_multi(octetSet, 1);
			bcm2835_gpio_write_multi(octetClear, 0);

			bcm2835_gpio_set(clockPin);
			bcm2835_delayMicroseconds(highDelay);

		}
	}
	else
	{
		for (x = 0; x < length; x++)
		{
			data = buffer[x];

			octetSet = (((data & 1) ? 1 : 0) << pinList[0]) |
				(((data & 2) ? 1 : 0) << pinList[1]) |
				(((data & 4) ? 1 : 0) << pinList[2]) |
				(((data & 8) ? 1 : 0) << pinList[3]) |
				(((data & 16) ? 1 : 0) << pinList[4]) |
				(((data & 32) ? 1 : 0) << pinList[5]) |
				(((data & 64) ? 1 : 0) << pinList[6]) |
				(((data & 128) ? 1 : 0) << pinList[7]);

			octetClear = (((data & 1) ? 0 : 1) << pinList[0]) |
				(((data & 2) ? 0 : 1) << pinList[1]) |
				(((data & 4) ? 0 : 1) << pinList[2]) |
				(((data & 8) ? 0 : 1) << pinList[3]) |
				(((data & 16) ? 0 : 1) << pinList[4]) |
				(((data & 32) ? 0 : 1) << pinList[5]) |
				(((data & 64) ? 0 : 1) << pinList[6]) |
				(((data & 128) ? 0 : 1) << pinList[7]);

			bcm2835_gpio_set(clockPin);
			bcm2835_delayMicroseconds(lowDelay);

			bcm2835_gpio_write_multi(octetSet, 1);
			bcm2835_gpio_write_multi(octetClear, 0);

			bcm2835_gpio_clr(clockPin);
			bcm2835_delayMicroseconds(highDelay);
		}

	}

}



static void *eventDetectorThread(void *arg)
{

	eventData* e = &eventDescriptors[(int)arg];

	struct pollfd descriptors[2];

	descriptors[0].fd = e->pipeFd[1];
	descriptors[0].events = POLLIN | POLLPRI;
	descriptors[1].fd = e->dataFd;
	descriptors[1].events = POLLPRI;

	int pollResult;
	uint8_t readValue;

	while (1)
	{
		descriptors[0].revents = 0;
		descriptors[1].revents = 0;

		pollResult = poll(descriptors, 2, -1);

		if (pollResult > 0)
		{
			if (descriptors[0].revents != 0)// || (pollResult & (POLLERR | POLLHUP | POLLNVAL)))
			{
				close(e->dataFd);
				close(e->pipeFd[0]);
				close(e->pipeFd[1]);
				e->callback(descriptors[0].revents != 0  ? 0xFE : 0xFF);
				e->callback = NULL;
				return (void*)1;
			}
			
			read(e->dataFd, &readValue, 1);
			lseek(e->dataFd, 0, SEEK_SET);

			e->callback(readValue);			
		}
	}

	return 0;
}

uint8_t setPinEventDetector(uint8_t pin, void(*callback)(uint8_t))
{
	char fname[64];
	FILE* fd;

	if ((fd = fopen("/sys/class/gpio/export", "w")) == NULL)
		return 1;

	fprintf(fd, "%d\n", pin);
	fclose(fd);

	sprintf(fname, "/sys/class/gpio/gpio%d/value", pin);

	eventDescriptors[pin].dataFd = open(fname, O_RDWR);

	if (eventDescriptors[pin].dataFd < 1)
		return 2;

	if (pipe(eventDescriptors[pin].pipeFd) == -1)
	{
		close(eventDescriptors[pin].dataFd);
		return 3;
	}

	eventDescriptors[pin].callback = callback;

	if (pthread_create(&eventDescriptors[pin].threadId, NULL, eventDetectorThread, (void*)(int)pin) != 0)
	{
		close(eventDescriptors[pin].dataFd);
		close(eventDescriptors[pin].pipeFd[0]);
		close(eventDescriptors[pin].pipeFd[1]);

		return 4;
	}

	return 0;

}

uint8_t prepareManualEventDetector(uint8_t pin)
{

	char fname[64];
	FILE* fd;

	if ((fd = fopen("/sys/class/gpio/export", "w")) == NULL)
		return 1;

	fprintf(fd, "%d\n", pin);
	fclose(fd);

	sprintf(fname, "/sys/class/gpio/gpio%d/value", pin);

	eventDescriptors[pin].dataFd = open(fname, O_RDWR);

	if (eventDescriptors[pin].dataFd < 1)
		return 2;

	if (pipe(eventDescriptors[pin].pipeFd) == -1)
	{
		close(eventDescriptors[pin].dataFd);
		return 3;
	}

	eventDescriptors[pin].descriptors[0].fd = eventDescriptors[pin].pipeFd[1];
	eventDescriptors[pin].descriptors[0].events = POLLIN | POLLPRI;
	eventDescriptors[pin].descriptors[1].fd = eventDescriptors[pin].dataFd;
	eventDescriptors[pin].descriptors[1].events = POLLPRI;

	read(eventDescriptors[pin].dataFd, fname, 1);
	lseek(eventDescriptors[pin].dataFd, 0, SEEK_SET);

	return 0;
}

uint8_t manualEventDetect(uint8_t pin)
{

	eventDescriptors[pin].descriptors[0].revents = 0;
	eventDescriptors[pin].descriptors[1].revents = 0;
	
	int pollResult;
	uint8_t readValue;

	pollResult = poll(eventDescriptors[pin].descriptors, 2, -1);

	if (pollResult > 0)
	{
		if (eventDescriptors[pin].descriptors[0].revents != 0)// || (pollResult & (POLLERR | POLLHUP | POLLNVAL)))
		{
			close(eventDescriptors[pin].dataFd);
			close(eventDescriptors[pin].pipeFd[0]);
			close(eventDescriptors[pin].pipeFd[1]);
			return 0xFF;
		}

		read(eventDescriptors[pin].dataFd, &readValue, 1);
		lseek(eventDescriptors[pin].dataFd, 0, SEEK_SET);

		return readValue;
	}

	close(eventDescriptors[pin].dataFd);
	close(eventDescriptors[pin].pipeFd[0]);
	close(eventDescriptors[pin].pipeFd[1]);
	return 0xFF;

}

void clearPinEventDetector(uint8_t pin)
{

	close(eventDescriptors[pin].dataFd);
	close(eventDescriptors[pin].pipeFd[0]);
	close(eventDescriptors[pin].pipeFd[1]);

}

uint8_t setDetectEdge(uint8_t pin, uint8_t mode)
{

	if (mode > 3)
		return 0xFF;

	FILE* fd;

	if ((fd = fopen("/sys/class/gpio/export", "w")) == NULL)
		return 1;

	fprintf(fd, "%d\n", pin);
	fclose(fd);

	char fname[64];
	sprintf(fname, "/sys/class/gpio/gpio%d/edge", pin);

	if ((fd = fopen(fname, "w")) == NULL)
		return 2;

	switch (mode)
	{
	case 0:
		fputs("none\n", fd);
		break;
	case 1:
		fputs("rising\n", fd);
		break;
	case 2:
		fputs("falling\n", fd);
		break;
	case 3:
		fputs("both\n", fd);
		break;
	}

	fclose(fd);

	return 0;
}

uint8_t exportPin(uint8_t pin)
{
	FILE* fd;

	if ((fd = fopen("/sys/class/gpio/export", "w")) == NULL)
		return 1;

	fprintf(fd, "%d\n", pin);
	fclose(fd);

	return 0;
}