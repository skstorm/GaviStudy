#include "LambdaTest.h"

template< typename Func>
void Test(Func func)
{
	func();
}



void LambdaFunc1()
{
//	std::cout<<std::endl<<"////////////  LambdaFunc1  /////////////"<<std::endl;
	gLamdaStart( "LambdaFunc1" );
	
	auto lambda = [] (int n) { std::cout<<std::endl<<"lamdbda : "<<n<<std::endl; };
	
	//람다는 함수의 인자로 사용 가능하다.
	lambda(1);
	auto func1 = [] { std::cout<<"Gunz2 is Greate Game!!"<<std::endl;};
	Test(func1);


	//람다는 인자를 넘겨줄수 있다.
	lambda(2);
	auto func2 = [] (int n) { std::cout<<"Number : "<<n<<std::endl; };
	func2(333);
	func2(777);


	//람다는 값을 반환 할수 있다.
	lambda(3);
	auto func3 = [] { return 3.14;};
	auto func4 = [](float f) { return f;};
	auto func5 = [] () -> float {return 3.14;}; //반환타입 명시적으로 반환!

	float f1 = func3();
	float f2 = func4(3.14f);
	float f3 = func5();
	std::cout<<f1<<" "<<f2<<" "<<f3<<std::endl;
	gLamdaEnd();

}