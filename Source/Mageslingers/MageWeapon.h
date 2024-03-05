// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "WeaponObject.h"
#include "MageWeapon.generated.h"

UCLASS()
class MAGESLINGERS_API AMageWeapon : public AWeaponObject
{
	GENERATED_BODY()
	
public:	
	// Sets default values for this actor's properties
	AMageWeapon();

protected:
	// Called when the game starts or when spawned
	virtual void BeginPlay() override;

public:	
	// Called every frame
	virtual void Tick(float DeltaTime) override;

};