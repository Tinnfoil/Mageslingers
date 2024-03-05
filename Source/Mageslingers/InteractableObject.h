// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "Interactable.h"
#include "BaseObject.h"
#include "InteractableObject.generated.h"

UCLASS()
class MAGESLINGERS_API AInteractableObject : public ABaseObject, public IInteractable
{
	GENERATED_BODY()
	
public:	
	// Sets default values for this actor's properties
	AInteractableObject();

protected:
	// Called when the game starts or when spawned
	virtual void BeginPlay() override;

public:	
	// Called every frame
	virtual void Tick(float DeltaTime) override;

	virtual void StartInteraction_Implementation(AActor* InteractingActor) override;
	
	virtual void EndInteraction_Implementation(AActor* InteractingActor) override;

	UPROPERTY(EditAnywhere)
	FString InteractText = "Interact[E]";

	virtual FString GetInteractText_Implementation() override;
};