#ifndef _LAMBDATEST_HEADER_
#define _LAMBDATEST_HEADER_

#include <iostream>

auto gLamdaStart = [] ( std::string str ) { std::cout<<std::endl<<"/////////////  "<<str.c_str()<<"  /////////////"<<std::endl; } ;
auto gLamdaEnd = [] () { std::cout<<std::endl<<"///----------------------------------///"<<std::endl; } ;

auto gggaaa = 11;

void LambdaFunc1();
void LambdaFunc2();
void LambdaAndClass();
void LambdaFindIf();
void LambdaFinal();

#endif