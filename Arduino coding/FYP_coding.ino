#define sensor_pin 4
#define FORCE_SENSOR_PIN 36
int read_ADC = 0;

void setup(){
Serial.begin(115200);
pinMode(sensor_pin, INPUT);
delay(20);
}
 
void loop(){
read_ADC = analogRead(sensor_pin);
Serial.print(",");//EMG
Serial.println(read_ADC);
delay(20);


int analogReading = analogRead(FORCE_SENSOR_PIN);
Serial.print(",");//FSR
Serial.print(analogReading); // print the raw analog reading

delay(20);
}