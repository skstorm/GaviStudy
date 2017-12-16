#include "LambdaTest.h"

#include <vector>
#include <algorithm>

using namespace std;

class NetWork
{
public :
	NetWork()
	{
		SendPackets.push_back(10);
		SendPackets.push_back(32);
		SendPackets.push_back(24);
	}

	void AllSend() const
	{
		for_each(SendPackets.begin(), SendPackets.end(), [this] (int i) { Send(i); } );
	}

private :
	vector<int > SendPackets;

	void Send(int PacketIndex) const
	{
		cout<<"Send Packet Index : "<<PacketIndex<<endl;
	}
};

void LambdaAndClass()
{
	gLamdaStart( "LambdaAndClass" );
	NetWork().AllSend();
	gLamdaEnd();
}
