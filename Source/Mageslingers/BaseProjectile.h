// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "BaseObject.h"
#include "GameFramework/ProjectileMovementComponent.h"
#include "BaseProjectile.generated.h"

UCLASS()
class MAGESLINGERS_API ABaseProjectile : public ABaseObject
{
	GENERATED_BODY()
	
public:	
	// Sets default values for this actor's properties
	ABaseProjectile();

protected:
	// Called when the game starts or when spawned
	virtual void BeginPlay() override;

public:	
	// Called every frame
	virtual void Tick(float DeltaTime) override;


	UPROPERTY(EditAnywhere, BlueprintReadWrite);
	float BaseLifeTime = 10;

	UPROPERTY(EditAnywhere, BlueprintReadWrite);
	bool DestroyOnHit = true;

	UPROPERTY(EditAnywhere, BlueprintReadWrite);
	bool HasGravity = true;

};
