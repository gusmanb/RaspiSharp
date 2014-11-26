RaspiSharp
==========

RaspBerry Pi interface for Mono

Revamped version with tons of improvments:

1-Dual API interface, the old one which is intended to create programs and the new "software" interface.
2-Modified BCM library with native functions for fast data transfers
3-More virtualized port types.

Ok, so the new interface is really made for the new project I'm working on: RaspiStudio.

The idea behind RaspiStudio is to create nearly anything you want (and your RPi allows)
in a visual fashion, just connect outputs to inputs and it's done.

If you want to programatically use that interface you will see it's really easy, every element
is derived of RaspElement and implements Inputs and Outputs.

An Input is a function which expects a sender and a *EventArgs, it can be Signal (boolean) Byte or Buffer.
An Output is an event of type SignalEventArgs, ByteEventArgs or BufferEventArgs.

As you can see, any output can be connected to an input of the appropiate type, so it's really easy to use.

Also there are lots of converters/modifiers of events to allow per example send a buffer when a signal is received
or to send a signal when a byte with a concrete value is received.


THIS IS A WORK IN PROGRESS, EXPECT FAILS.

All released under NoLicense unless specified in the porject Readme

Have fun!
