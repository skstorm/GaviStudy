#include "LambdaTest.h"
#include <algorithm>
#include <functional>
#include <vector>
#include <string>

using namespace std;

//���ٸ� ��ȯ�ϴ� �Լ�
function< void() > f()
{
	string str("lambda");
	return [=] { cout<<"Hello, "<<str<<endl; };
}

void LambdaFinal()
{
	gLamdaStart( "LambdaFinal" );
	/////////////////////
	cout<<"���ٽ��� STL �����̳ʿ� ����"<<endl;
	vector<function<int ()>> v;
	v.push_back( [] { return 100; });
	v.push_back( [] { return 200; } );

	cout<<v[0]()<<endl;
	cout<<v[1]()<<endl;
	//--------------------//

	///////////////////
	cout<<endl<<"���ٸ� ��ȯ�ϴ� �Լ�"<<endl;
	auto func = f();
	func();
	f() ();
	//--------------//

	/////////////////////
	cout<<endl<<"���ٿ����� ���"<<endl;
	function<int(int)> fact = [&fact](int x){
		return x== 0?1:x*fact(x-1);
	};

	cout<<fact(3) <<endl;
	//----------------//
	
	gLamdaEnd();
}   