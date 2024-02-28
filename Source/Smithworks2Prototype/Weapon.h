// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "UObject/Interface.h"
#include "Weapon.generated.h"

// This class does not need to be modified.
UINTERFACE(MinimalAPI)
class UWeapon : public UInterface
{
	GENERATED_BODY()
};

/**
 * 
 */
class SMITHWORKS2PROTOTYPE_API IWeapon
{
	GENERATED_BODY()

	// Add interface functions to this class. This is the class that will be inherited to implement this interface.
public:

	UFUNCTION(BlueprintNativeEvent, BlueprintCallable)
	void ReadyWeapon();

	UFUNCTION(BlueprintNativeEvent, BlueprintCallable)
	void UnreadyWeapon();

	UFUNCTION(BlueprintNativeEvent, BlueprintCallable)
	void ChargeWeapon();

	UFUNCTION(BlueprintNativeEvent, BlueprintCallable)
	void FireWeapon();

	UFUNCTION(BlueprintNativeEvent, BlueprintCallable)
	bool IsWeaponReady();

	UFUNCTION(BlueprintNativeEvent, BlueprintCallable)
	bool IsWeaponCharging() const;

};