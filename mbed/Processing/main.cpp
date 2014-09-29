/*
Ball detector for openpool (openpool.cc)
Originally wrote by Kazunori Ogasawara, taken over by Shinya Shimizu (@kakenman)
July 14, 2013 updated by @kakenman
August 7, 2013 updated by @takashyx
*/
#include "mbed.h"

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
                    pc.printf("%u",fallCount[i]);
                    if(i<5)
                    {
                        pc.printf(",");
                    }                    
                    fallCount[i] = 0;
                }
                pc.printf("\n");
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
