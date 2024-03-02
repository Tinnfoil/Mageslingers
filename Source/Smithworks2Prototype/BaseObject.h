// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "BaseObject.generated.h"

UCLASS()
class SMITHWORKS2PROTOTYPE_API ABaseObject : public AActor
{
	GENERATED_BODY()
	
public:	
	// Sets default values for this actor's properties
	ABaseObject();

private:

protected:
	
	UPROPERTY(EditAnywhere, BlueprintReadWrite)
	UStaticMeshComponent* BaseMesh;

	// Called when the game starts or when spawned
	virtual void BeginPlay() override;

public:	

	// Called every frame
	virtual void Tick(float DeltaTime) override;
};
