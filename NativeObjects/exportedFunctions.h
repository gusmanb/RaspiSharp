#ifndef exportedFunctions_h__
#define exportedFunctions_h__

#include <stdio.h>
#include <poll.h>
#include <pthread.h>
#include <fcntl.h>
#include <unistd.h>
#include  "bcm2835.h"

typedef struct

{

	int pipeFd[2];
	int dataFd;
	void(*callback)(uint8_t);
	struct pollfd descriptors[2];
	pthread_t threadId;

} eventData;



extern uint32_t bcm2835_gpio_full_eds();
extern void bcm2835_gpio_full_set_eds(uint8_t pinMask);

extern uint8_t readBitBangByte(uint8_t pin, uint8_t clockPin, uint8_t polarity, uint32_t lowDelay, uint32_t highDelay);
extern void readBitBangBuffer(uint8_t* buffer, uint32_t length, uint8_t pin, uint8_t clockPin, uint8_t polarity, uint32_t lowDelay, uint32_t highDelay);
extern void writeBitBangByte(uint8_t data, uint8_t pin, uint8_t clockPin, uint8_t polarity, uint32_t lowDelay, uint32_t highDelay);
extern void writeBitBangBuffer(uint8_t* buffer, uint32_t length, uint8_t pin, uint8_t clockPin, uint8_t polarity, uint32_t lowDelay, uint32_t highDelay);

extern uint8_t readNibbleByte(uint8_t pin0, uint8_t pin1, uint8_t pin2, uint8_t pin3, uint8_t clockPin, uint8_t polarity, uint32_t lowDelay, uint32_t highDelay);
extern void readNibbleBuffer(uint8_t* buffer, uint32_t length, uint8_t pin0, uint8_t pin1, uint8_t pin2, uint8_t pin3, uint8_t clockPin, uint8_t polarity, uint32_t lowDelay, uint32_t highDelay);
extern void writeNibbleByte(uint8_t data, uint8_t pin0, uint8_t pin1, uint8_t pin2, uint8_t pin3, uint8_t clockPin, uint8_t polarity, uint32_t lowDelay, uint32_t highDelay);
extern void writeNibbleBuffer(uint8_t* buffer, uint32_t length, uint8_t pin0, uint8_t pin1, uint8_t pin2, uint8_t pin3, uint8_t clockPin, uint8_t polarity, uint32_t lowDelay, uint32_t highDelay);

extern uint8_t readOctetByte(uint8_t pinList[8], uint8_t clockPin, uint8_t polarity, uint32_t lowDelay, uint32_t highDelay);
extern void readOctetBuffer(uint8_t* buffer, uint32_t length, uint8_t pinList[8], uint8_t clockPin, uint8_t polarity, uint32_t lowDelay, uint32_t highDelay);
extern void writeOctetByte(uint8_t data, uint8_t pinList[8], uint8_t clockPin, uint8_t polarity, uint32_t lowDelay, uint32_t highDelay);
extern void writeOctetBuffer(uint8_t* buffer, uint32_t length, uint8_t pinList[8], uint8_t clockPin, uint8_t polarity, uint32_t lowDelay, uint32_t highDelay);

extern uint8_t setPinEventDetector(uint8_t pin, void(*callback)(uint8_t));
extern uint8_t prepareManualEventDetector(uint8_t pin);
extern uint8_t manualEventDetect(uint8_t pin);

extern void clearPinEventDetector(uint8_t pin);

extern uint8_t setDetectEdge(uint8_t pin, uint8_t mode);
extern uint8_t exportPin(uint8_t pin);

#endif  // exportedFunctions_h__