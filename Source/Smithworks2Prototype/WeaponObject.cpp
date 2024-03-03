// Fill out your copyright notice in the Description page of Project Settings.


#include "WeaponObject.h"

// Sets default values
AWeaponObject::AWeaponObject()
{
 	// Set this actor to call Tick() every frame.  You can turn this off to improve performance if you don't need it.
	PrimaryActorTick.bCanEverTick = true;
	CurrChargeTime = ChargeTime;
}

// Called when the game starts or when spawned
void AWeaponObject::BeginPlay()
{
	Super::BeginPlay();

}

// Called every frame
void AWeaponObject::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);

}

void AWeaponObject::ReadyWeapon_Implementation()
{
	if(GEngine)
    	GEngine->AddOnScreenDebugMessage(-1, 15.0f, FColor::Yellow, TEXT("ReadyWeapon_Implementation Base"));	
}
void AWeaponObject::UnreadyWeapon_Implementation()
{
	if(GEngine)
    	GEngine->AddOnScreenDebugMessage(-1, 15.0f, FColor::Yellow, TEXT("UnreadyWeapon_Implementation Base"));	
}
void AWeaponObject::ChargeWeapon_Implementation()
{
	if(GEngine)
    	GEngine->AddOnScreenDebugMessage(-1, 15.0f, FColor::Yellow, TEXT("ChargeWeapon_Implementation Base"));	
}
void AWeaponObject::FireWeapon_Implementation()
{
	if(GEngine)
    	GEngine->AddOnScreenDebugMessage(-1, 15.0f, FColor::Yellow, TEXT("FireWeapon_Implementation Base"));	
}

bool AWeaponObject::IsWeaponCharging_Implementation() const
{
	if(GEngine)
    	GEngine->AddOnScreenDebugMessage(-1, 15.0f, FColor::Yellow, TEXT("IsWeaponCharging Base"));	

	return CurrChargeTime < ChargeTime;
}


