#pragma once

#include <list>

class ReferencedObjectConsumer
{
public:
	ReferencedObjectConsumer();
	~ReferencedObjectConsumer();

	void UsingReferencedObject();
};



#include "ReferencedObjectConsumer.h"

#include <memory>

#include "../Providers/ProviderClass.h"

ReferencedObjectConsumer::ReferencedObjectConsumer()
{
}

ReferencedObjectConsumer::~ReferencedObjectConsumer()
{

}

void ReferencedObjectConsumer::UsingReferencedObject()
{
	ProviderClass object;
	ProviderClass& reference = object;
	reference.PublicMethodA();
	reference.PublicMethodB();
}
