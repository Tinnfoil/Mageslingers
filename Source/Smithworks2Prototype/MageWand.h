// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "MageWeapon.h"
#include "MageWand.generated.h"

UCLASS()
class SMITHWORKS2PROTOTYPE_API AMageWand : public AMageWeapon
{
	GENERATED_BODY()
	
public:	
	// Sets default values for this actor's properties
	AMageWand();

protected:
	// Called when the game starts or when spawned
	virtual void BeginPlay() override;

public:	
	// Called every frame
	virtual void Tick(float DeltaTime) override;

};
