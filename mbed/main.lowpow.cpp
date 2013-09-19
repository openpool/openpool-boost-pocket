/*
Ball detector for openpool (openpool.cc)
Originally wrote by Kazunori Ogasawara, taken over by Shinya Shimizu (@kakenman)
July 14, 2013 updated by @kakenman
Sep 18, 2013 power saving function added by @kakenman. To use this version, you have to install custom firmware described below.
http://mbed.org/users/simon/notebook/interface-powerdown/

*/
#include "mbed.h"

#include "PowerControl/PowerControl.h"
#include "PowerControl/EthernetPowerControl.h"
// Need PowerControl *.h files from this URL
// http://mbed.org/users/no2chem/notebook/mbed-power-controlconsumption/


// Function to power down magic USB interface chip with new firmware
#define USR_POWERDOWN    (0x104)
int semihost_powerdown()
{
    uint32_t arg;
    return __semihost(USR_POWERDOWN, &arg);
}


DigitalOut myled0(LED1);
DigitalOut myled1(LED2);
DigitalOut myled2(LED3);
DigitalOut myled3(LED4);

DigitalIn fallSensor0(p30);
DigitalIn fallSensor1(p29);
DigitalIn fallSensor2(p28);
//DigitalIn fallSensor3(p15);
DigitalIn fallSensor3(p13);
DigitalIn fallSensor4(p12);
DigitalIn fallSensor5(p11);

Ticker fallTimer;

// You can choose Seraial communication via USB or via XBEE
//Serial pc(USBTX,USBRX);
Serial pc(p9,p10);
unsigned int fallSenseFlag;

void setFallSenseFlag()
{
    fallSenseFlag = 1;
}

int main()
{

/////////////////////////////////// power saving setting start
    int result;
// Normal mbed power level for this setup is around 690mW
// assuming 5V used on Vin pin
// If you don't need networking...
// Power down Ethernet interface - saves around 175mW
// Also need to unplug network cable - just a cable sucks power
    PHY_PowerDown();

// If you don't need the PC host USB interface....
// Power down magic USB interface chip - saves around 150mW
// Needs new firmware (URL below) and USB cable not connected
// http://mbed.org/users/simon/notebook/interface-powerdown/
// Supply power to mbed using Vin pin
    result = semihost_powerdown();
// Power consumption is now around half

// Turn off clock enables on unused I/O Peripherals (UARTs, Timers, PWM, SPI, CAN, I2C, A/D...)
// To save just a tiny bit more power - most are already off by default in this short code example
// See PowerControl.h for I/O device bit assignments
// Only UARTS and TIMERS are enabled
    Peripheral_PowerDown(0xFC3E7FE1);
/////////////////////////////////// power saving setting end


    int i;
    unsigned int fallState[6] = {1,1,1,1,1,1};
    unsigned int lastFallState[6] = {1,1,1,1,1,1};
    unsigned int fallCount[6] = {0,0,0,0,0,0};
    fallSenseFlag = 0;  //deflag fallSenseFlag
    char buff;
    pc.baud(115200);



    while(1) {                           //wait for start command
        if(pc.readable()) {
            if(pc.getc() == 's') {
                pc.printf("measurement start\n");
                myled0 = 1;
                myled1 = 1;
                myled2 = 1;
                myled3 = 1;
                wait(1);
                myled0 = 0;
                myled1 = 0;
                myled2 = 0;
                myled3 = 0;
                break;
            }
        }
    }
    fallTimer.attach_us(&setFallSenseFlag,100);  //sensing timer start

    while(1) {
        if(pc.readable()) {               //if serial buffer is readable
            buff = pc.getc();
            if(buff == 'r') {        //check command
                for(i=0; i<6; i++) {      //send data
                    pc.printf("%u\n",fallCount[i]);
                    fallCount[i] = 0;
                }
            }
            if(buff == 's') {
                pc.printf("measurement start\n");
                myled0 = 1;
                myled1 = 1;
                myled2 = 1;
                myled3 = 1;
                wait(1);
                myled0 = 0;
                myled1 = 0;
                myled2 = 0;
                myled3 = 0;
                for(i=0; i<6; i++) {
                    fallCount[i] = 0;
                }
            }
        }
        if(fallSenseFlag == 1) {
            fallState[0] = fallSensor0;     //read current sensors state
            fallState[1] = fallSensor1;
            fallState[2] = fallSensor2;
            fallState[3] = fallSensor3;
            fallState[4] = fallSensor4;
            fallState[5] = fallSensor5;

            for(i=0; i<6; i++) {            //if ball fall, increment fall count
                if((lastFallState[i] == 1) && (fallState[i] == 0)) {
                    fallCount[i]++;
                }
                lastFallState[i] = fallState[i];    //update last state
            }

            if(fallCount[0] > 0) {
                myled0 = 1;
            } else {
                myled0 = 0;
            }
            if(fallCount[1] > 0) {
                myled1 = 1;
            } else {
                myled1 = 0;
            }
            if(fallCount[2] > 0) {
                myled2 = 1;
            } else {
                myled2 = 0;
            }
            if(fallCount[3] > 0) {
                myled3 = 1;
            } else {
                myled3 = 0;
            }


            /*
            if(fallCount[1] > 0){
                myled1 = 1;
            }else{
                myled1 = 0;
            }
            if(fallCount[2] > 0){
                myled2 = 1;
            }else{
                myled2 = 0;
            }
            if(fallCount[3] > 0){
                myled3 = 1;
            }else{
                myled3 = 0;
            }*/
            fallSenseFlag = 0;
        }

    }
}
