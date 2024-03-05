// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "InteractableObject.h"
#include "Pickupable.h"
#include "PickupableObject.generated.h"

UCLASS()
class MAGESLINGERS_API APickupableObject : public AInteractableObject, public IPickupable
{
	GENERATED_BODY()
	
public:	
	// Sets default values for this actor's properties
	APickupableObject();

protected:
	// Called when the game starts or when spawned
	virtual void BeginPlay() override;

public:	
	// Called every frame
	virtual void Tick(float DeltaTime) override;

	virtual void StartInteraction_Implementation(AActor* InteractingActor) override;

	virtual void EndInteraction_Implementation(AActor* InteractingActor) override;

	virtual void Pickup_Implementation(AActor* Actor) override;

	virtual void Drop_Implementation(AActor* Actor) override;

};
