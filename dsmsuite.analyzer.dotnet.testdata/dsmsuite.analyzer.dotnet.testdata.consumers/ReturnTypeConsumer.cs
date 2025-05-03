#pragma once

#include <list>
#include <memory>

#include "../Providers/ProviderGenericClass.h"
#include "../Providers/ProviderClass.h"
#include "../Providers/ProviderStruct.h"
#include "../Providers/ProviderUnion.h"
#include "../Providers/ProviderEnum.h"

class ReturnTypeConsumer
{
public:
	ReturnTypeConsumer();
	~ReturnTypeConsumer();

	void MethodWithReturnTypeVoid();
	int MethodWithReturnTypeInt();
	ProviderEnum MethodWithGenericReturnTypeEnum();
	ProviderStruct MethodWithGenericReturnTypeStruct();
	ProviderUnion MethodWithGenericReturnTypeUnion();
	ProviderClass MethodWithReturnTypeClass();
	ProviderClass* MethodWithReturnTypeClassPtr();
	std::unique_ptr<ProviderClass> MethodWithReturnTypeClassUniquePtr();
	std::list<ProviderStdListTemplateArgument> MethodWithStdListReturnType();
	ProviderGenericClass<ProviderTemplateArgument1, ProviderTemplateArgument2> MethodWithGenericClassReturnType();
};


#include "ReturnTypeConsumer.h"

#include <memory>

ReturnTypeConsumer::ReturnTypeConsumer()
{
}

ReturnTypeConsumer::~ReturnTypeConsumer()
{

}

void ReturnTypeConsumer::MethodWithReturnTypeVoid()
{

}

int ReturnTypeConsumer::MethodWithReturnTypeInt()
{
	return 0;
}

ProviderEnum ReturnTypeConsumer::MethodWithGenericReturnTypeEnum()
{
	ProviderEnum value = ProviderEnum::enum_val1;
	return value;
}

ProviderStruct ReturnTypeConsumer::MethodWithGenericReturnTypeStruct()
{
	ProviderStruct value;
	return value;
}

ProviderUnion ReturnTypeConsumer::MethodWithGenericReturnTypeUnion()
{
	ProviderUnion value;
	return value;
}

ProviderClass ReturnTypeConsumer::MethodWithReturnTypeClass()
{
	ProviderClass value;
	return value;
}

ProviderClass* ReturnTypeConsumer::MethodWithReturnTypeClassPtr()
{
	ProviderClass* pValue = new ProviderClass();
	return pValue;
}

std::unique_ptr<ProviderClass> ReturnTypeConsumer::MethodWithReturnTypeClassUniquePtr()
{
	std::unique_ptr<ProviderClass> pValue = std::make_unique<ProviderClass>();
	return pValue;
}

std::list<ProviderStdListTemplateArgument> ReturnTypeConsumer::MethodWithStdListReturnType()
{
	std::list<ProviderStdListTemplateArgument> value;
	return value;
}

ProviderGenericClass<ProviderTemplateArgument1, ProviderTemplateArgument2> ReturnTypeConsumer::MethodWithGenericClassReturnType()
{
	ProviderGenericClass<ProviderTemplateArgument1, ProviderTemplateArgument2> value;
	return value;
}




