// Fill out your copyright notice in the Description page of Project Settings.


#include "BaseProjectile.h"

// Sets default values
ABaseProjectile::ABaseProjectile()
{
 	// Set this actor to call Tick() every frame.  You can turn this off to improve performance if you don't need it.
	PrimaryActorTick.bCanEverTick = true;

	BaseMesh->SetEnableGravity(HasGravity);
	BaseMesh->SetGenerateOverlapEvents(true);
}

// Called when the game starts or when spawned
void ABaseProjectile::BeginPlay()
{
	Super::BeginPlay();
	
}

// Called every frame
void ABaseProjectile::Tick(float DeltaTime)
{
	BaseLifeTime -= DeltaTime;

	if(BaseLifeTime <= 0)
	{
		Destroy();
	}

	Super::Tick(DeltaTime);

}

