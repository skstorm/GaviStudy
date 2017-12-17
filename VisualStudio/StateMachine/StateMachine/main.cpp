#include <iostream>
#include <stdio.h>
#include "CTask.h"

#define END_FRAME 20

using namespace std;

void main()
{
	cout<<"mokue!!"<<endl;

	CMp3Task mp3Task;

	for(int i=0;i<END_FRAME; ++i)
	{
		mp3Task.Update();
		Sleep(800);
	}
}