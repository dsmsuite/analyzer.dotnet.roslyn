#pragma once

#include <list>

class BaseClassConsumer
{
public:
	BaseClassConsumer();
	~BaseClassConsumer();

	void MethodCallingAbstractFunctionsInClassHierarchy();
	void MethodCallingConcreteFunctionsInClassHierarchy();
};

#include "BaseClassConsumer.h"

#include "../Providers/ProviderAbstractClass.h"
#include "../Providers/ProviderBaseClass1.h"

BaseClassConsumer::BaseClassConsumer()
{
}

BaseClassConsumer::~BaseClassConsumer()
{
}

void  BaseClassConsumer::MethodCallingAbstractFunctionsInClassHierarchy()
{
	ProviderAbstractClass* pAbstractClass = new ProviderBaseClass1();
	pAbstractClass->AbstractBaseMethod();
}

void  BaseClassConsumer::MethodCallingConcreteFunctionsInClassHierarchy()
{
	ProviderBaseClass1* pBaseClass1 = new ProviderBaseClass1();
	pBaseClass1->AbstractBaseMethod();
	pBaseClass1->ConcreteBaseMethod();
}


