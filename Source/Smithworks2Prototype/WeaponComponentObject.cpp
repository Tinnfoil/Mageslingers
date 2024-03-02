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

	SetWeaponSockets();
}

void AWeaponComponentObject:: SetWeaponSockets()
{
	TArray<FName> SocketNames = BaseMesh->GetAllSocketNames();
	for(FName socketName : SocketNames)
	{
		//if(GEngine)
			//GEngine->AddOnScreenDebugMessage(-1, 15.0f, FColor::Yellow, FString::Printf(TEXT("Looking Socket: %s"), *socketName.ToString()));	
		
		if(socketName.ToString().Contains("Gem Socket"))
		{
			GemSocketNames.Add(socketName);
		}
	}

	//if(GEngine)
		//GEngine->AddOnScreenDebugMessage(-1, 15.0f, FColor::Yellow, TEXT("Adding Gem Sockets"));	

	/*
	for(FName socketName : GemSocketNames)
	{
		if(GEngine)
			GEngine->AddOnScreenDebugMessage(-1, 15.0f, FColor::Yellow, FString::Printf(TEXT("Added Gem Socket: %s"), *socketName.ToString()));	
	}
	*/

}

// Called every frame
void AWeaponComponentObject::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);

}

