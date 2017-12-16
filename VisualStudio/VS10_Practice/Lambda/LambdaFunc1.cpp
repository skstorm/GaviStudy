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
	
	//���ٴ� �Լ��� ���ڷ� ��� �����ϴ�.
	lambda(1);
	auto func1 = [] { std::cout<<"Gunz2 is Greate Game!!"<<std::endl;};
	Test(func1);


	//���ٴ� ���ڸ� �Ѱ��ټ� �ִ�.
	lambda(2);
	auto func2 = [] (int n) { std::cout<<"Number : "<<n<<std::endl; };
	func2(333);
	func2(777);


	//���ٴ� ���� ��ȯ �Ҽ� �ִ�.
	lambda(3);
	auto func3 = [] { return 3.14;};
	auto func4 = [](float f) { return f;};
	auto func5 = [] () -> float {return 3.14;}; //��ȯŸ�� ��������� ��ȯ!

	float f1 = func3();
	float f2 = func4(3.14f);
	float f3 = func5();
	std::cout<<f1<<" "<<f2<<" "<<f3<<std::endl;
	gLamdaEnd();

}