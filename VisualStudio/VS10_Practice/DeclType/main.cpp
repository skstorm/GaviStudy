#include <iostream>

using namespace std;

template <typename T, typename U>   
auto f(T x, U y) -> decltype(x*y) { return x*y; }  

void main()
{
	int Hp = 3;
	decltype(Hp) NPCHp = 5;
	decltype(Hp + NPCHp) TotalHp = 7;
//	decltype(Hp*) pHp = &Hp;  예제랑 동일함에도 불구하고 안됨... ㅡㅡ;;;

	cout<<"Hp : "<<Hp<<endl;
	cout<<"NPCHp : "<<NPCHp<<endl;
	cout<<"TotalHp : "<<TotalHp<<endl;
//	cout<<"pHp : "<<*pHp<<endl;
	cout<<f(3,4.15f)<<endl;

	getchar();
}