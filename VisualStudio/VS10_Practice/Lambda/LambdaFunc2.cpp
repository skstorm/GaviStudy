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

	//변수 캡쳐!!!
	int TotalMoney1 = 0;
	int TotalBigMoney = 0;

	//Money가 1000 보다 크면 TotalBigMoney에 누적한다.
	for_each(Moneys.begin(), Moneys.end(),
		[&] (int Money)  
		{
			TotalMoney1 +=Money;
			if(Money > 1000) TotalBigMoney += Money;
		});
	cout<<"Total Money 1 : "<<TotalMoney1<<endl;
	cout<<"Total Big Money : "<<TotalBigMoney<<endl;

	/*
	복사로 캡쳐한 변수를 람다 내부에서 변경을 할때는
	mutable  키워드 사용
	[=]() mutable {std::cout<<x<<std::endl; x=200; } ();

	단 내부에서 변경한 외부 변수의 값은 람다를 벗어나면 변경전의 원래 값이 됨.
	*/
	gLamdaEnd();
}