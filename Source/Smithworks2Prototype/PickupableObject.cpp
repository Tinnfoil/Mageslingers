// Fill out your copyright notice in the Description page of Project Settings.


#include "PickupableObject.h"

// Sets default values
APickupableObject::APickupableObject()
{
 	// Set this actor to call Tick() every frame.  You can turn this off to improve performance if you don't need it.
	PrimaryActorTick.bCanEverTick = true;
}

// Called when the game starts or when spawned
void APickupableObject::BeginPlay()
{
	Super::BeginPlay();
	
}

// Called every frame
void APickupableObject::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);

}

void APickupableObject::StartInteraction_Implementation(AActor* InteractingActor)
{
	//if(GEngine)
    	//GEngine->AddOnScreenDebugMessage(-1, 3.0f, FColor::Yellow, TEXT("Interact Pickup Base"));	

	Execute_Pickup(this, InteractingActor);
	Pickup_Implementation(InteractingActor);
}

void APickupableObject::EndInteraction_Implementation(AActor* InteractingActor)
{
	if(GEngine)
    	GEngine->AddOnScreenDebugMessage(-1, 3.0f, FColor::Yellow, TEXT("Interact Drop Base"));	

	Execute_Drop(this, InteractingActor);
	Drop_Implementation(InteractingActor);
}

void APickupableObject::Pickup_Implementation(AActor* Actor)
{
	//if(GEngine)
    	//GEngine->AddOnScreenDebugMessage(-1, 3.0f, FColor::Yellow, TEXT("Pickup Base"));	
}

void APickupableObject::Drop_Implementation(AActor* Actor)
{
	if(GEngine)
    	GEngine->AddOnScreenDebugMessage(-1, 3.0f, FColor::Yellow, TEXT("Drop Base"));	
}

