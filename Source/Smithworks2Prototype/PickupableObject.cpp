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

void APickupableObject::StartInteraction_Implementation()
{
	if(GEngine)
    	GEngine->AddOnScreenDebugMessage(-1, 15.0f, FColor::Yellow, TEXT("Interact Pickup Base"));	

	Pickup_Implementation();
}

void APickupableObject::EndInteraction_Implementation()
{
	if(GEngine)
    	GEngine->AddOnScreenDebugMessage(-1, 15.0f, FColor::Yellow, TEXT("Interact Drop Base"));	

	Drop_Implementation();
}

void APickupableObject::Pickup_Implementation()
{
	if(GEngine)
    	GEngine->AddOnScreenDebugMessage(-1, 15.0f, FColor::Yellow, TEXT("Pickup Base"));	
}

void APickupableObject::Drop_Implementation()
{
	if(GEngine)
    	GEngine->AddOnScreenDebugMessage(-1, 15.0f, FColor::Yellow, TEXT("Drop Base"));	
}

