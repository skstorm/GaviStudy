#include "LambdaTest.h"
#include <vector>
#include <algorithm>

using namespace std;

void LambdaFunc2()
{	
	gLamdaStart("LambdaFunc2");
	vector<int> Moneys;
	Moneys.push_back(100);
	Moneys.push_back(4000);
	Moneys.push_back(50);
	Moneys.push_back(7); 

	//���� ĸ��!!!
	int TotalMoney1 = 0;
	int TotalBigMoney = 0;

	//Money�� 1000 ���� ũ�� TotalBigMoney�� �����Ѵ�.
	for_each(Moneys.begin(), Moneys.end(),
		[&] (int Money)  
		{
			TotalMoney1 +=Money;
			if(Money > 1000) TotalBigMoney += Money;
		});
	cout<<"Total Money 1 : "<<TotalMoney1<<endl;
	cout<<"Total Big Money : "<<TotalBigMoney<<endl;

	/*
	����� ĸ���� ������ ���� ���ο��� ������ �Ҷ���
	mutable  Ű���� ���
	[=]() mutable {std::cout<<x<<std::endl; x=200; } ();

	�� ���ο��� ������ �ܺ� ������ ���� ���ٸ� ����� �������� ���� ���� ��.
	*/
	gLamdaEnd();
}