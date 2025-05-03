#pragma once

#include <list>

class ClassInNamespaceConsumer
{
public:
	ClassInNamespaceConsumer();
	~ClassInNamespaceConsumer();

	void UsingExplicitNamespace();
	void UsingImplicitNamespace();
};

#include "ClassInNamespaceConsumer.h"

#include <memory>

#include "../Providers/ProviderClassInNamespace.h"

using namespace ImplicitProviderNamespace;

ClassInNamespaceConsumer::ClassInNamespaceConsumer()
{
}

ClassInNamespaceConsumer::~ClassInNamespaceConsumer()
{

}

void ClassInNamespaceConsumer::UsingExplicitNamespace()
{
	ExplicitProviderNamespace::ExplicitProviderClass object;
	object.PublicMethodA();
	object.PublicMethodB();
}

void ClassInNamespaceConsumer::UsingImplicitNamespace()
{
	ImplicitProviderClass object;
	object.PublicMethodA();
	object.PublicMethodB();
}

