// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "WeaponComponentObject.h"
#include "Weapon.h"
#include "WeaponObject.generated.h"

UCLASS()
class SMITHWORKS2PROTOTYPE_API AWeaponObject : public AWeaponComponentObject, public IWeapon
{
	GENERATED_BODY()
	
public:	
	// Sets default values for this actor's properties
	AWeaponObject();

private:

protected:
	// Called when the game starts or when spawned
	virtual void BeginPlay() override;

	float CurrChargeTime = 1;

public:	

	float ChargeTime = 1;
	// Called every frame
	virtual void Tick(float DeltaTime) override;

	virtual void ReadyWeapon_Implementation() override;

	virtual void UnreadyWeapon_Implementation() override;

	virtual void ChargeWeapon_Implementation() override;

	virtual void FireWeapon_Implementation() override;

	virtual bool IsWeaponCharging_Implementation() const override;

};
