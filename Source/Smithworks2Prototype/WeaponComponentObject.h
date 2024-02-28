// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "PickupableObject.h"
#include "WeaponComponentObject.generated.h"

UCLASS()
class SMITHWORKS2PROTOTYPE_API AWeaponComponentObject : public APickupableObject
{
	GENERATED_BODY()
	
public:	
	// Sets default values for this actor's properties
	AWeaponComponentObject();

private:
	//UPROPERTY(EditAnywhere)
	//TArray<USceneComponent*> Sockets;

	//UPROPERTY(EditAnywhere)
	//USceneComponent* SocketsParent;

	UFUNCTION(BlueprintCallable, Category = "Weapons/Sockets")
	void SetWeaponSockets(TArray<UStaticMeshSocket*> WeaponSockets);

protected:

	TArray<UStaticMeshSocket*> GemSockets;
	// Called when the game starts or when spawned
	virtual void BeginPlay() override;

public:	
	// Called every frame
	virtual void Tick(float DeltaTime) override;

};
