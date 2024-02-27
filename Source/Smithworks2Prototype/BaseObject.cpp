// Fill out your copyright notice in the Description page of Project Settings.


#include "BaseObject.h"

// Sets default values
ABaseObject::ABaseObject()
{
 	// Set this actor to call Tick() every frame.  You can turn this off to improve performance if you don't need it.
	PrimaryActorTick.bCanEverTick = true;

	BaseMesh = CreateDefaultSubobject<UStaticMeshComponent>(TEXT("Base Mesh"));

	RootComponent = BaseMesh;
	//BaseMesh->SetupAttachment(CapsuleComp);
}

// Called when the game starts or when spawned
void ABaseObject::BeginPlay()
{
	Super::BeginPlay();
	
}

// Called every frame
void ABaseObject::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);

}


