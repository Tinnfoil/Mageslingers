// Fill out your copyright notice in the Description page of Project Settings.


#include "WeaponComponentObject.h"

// Sets default values
AWeaponComponentObject::AWeaponComponentObject()
{
 	// Set this actor to call Tick() every frame.  You can turn this off to improve performance if you don't need it.
	PrimaryActorTick.bCanEverTick = true;

}

// Called when the game starts or when spawned
void AWeaponComponentObject::BeginPlay()
{
	Super::BeginPlay();
	
}

void AWeaponComponentObject:: SetWeaponSockets(TArray<UStaticMeshSocket*> WeaponSockets)
{
	GemSockets = WeaponSockets;
}

// Called every frame
void AWeaponComponentObject::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);

}

