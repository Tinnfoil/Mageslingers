// Fill out your copyright notice in the Description page of Project Settings.


#include "InteractableObject.h"

// Sets default values
AInteractableObject::AInteractableObject()
{
 	// Set this actor to call Tick() every frame.  You can turn this off to improve performance if you don't need it.
	PrimaryActorTick.bCanEverTick = true;

}

// Called when the game starts or when spawned
void AInteractableObject::BeginPlay()
{
	Super::BeginPlay();	
}

// Called every frame
void AInteractableObject::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);
}

void AInteractableObject::StartInteraction_Implementation()
{
	if(GEngine)
    	GEngine->AddOnScreenDebugMessage(-1, 15.0f, FColor::Yellow, TEXT("Interacted Base"));	

	// As Default implementation, we just end the interaction right away. We assume its a one time interaction
	EndInteraction_Implementation();
}

void AInteractableObject::EndInteraction_Implementation()
{
	if(GEngine)
    	GEngine->AddOnScreenDebugMessage(-1, 15.0f, FColor::Yellow, TEXT("End Base"));	
}

FString AInteractableObject::GetInteractText_Implementation()
{
	return InteractText;
}


