#include "LambdaTest.h"
#include <algorithm>
#include <functional>
#include <vector>
#include <string>

using namespace std;

//람다를 반환하는 함수
function< void() > f()
{
	string str("lambda");
	return [=] { cout<<"Hello, "<<str<<endl; };
}

void LambdaFinal()
{
	gLamdaStart( "LambdaFinal" );
	/////////////////////
	cout<<"람다식을 STL 컨테이너에 저장"<<endl;
	vector<function<int ()>> v;
	v.push_back( [] { return 100; });
	v.push_back( [] { return 200; } );

	cout<<v[0]()<<endl;
	cout<<v[1]()<<endl;
	//--------------------//

	///////////////////
	cout<<endl<<"람다를 반환하는 함수"<<endl;
	auto func = f();
	func();
	f() ();
	//--------------//

	/////////////////////
	cout<<endl<<"람다에서의 재귀"<<endl;
	function<int(int)> fact = [&fact](int x){
		return x== 0?1:x*fact(x-1);
	};

	cout<<fact(3) <<endl;
	//----------------//
	
	gLamdaEnd();
}   