// Fill out your copyright notice in the Description page of Project Settings.



#include "WeaponObject.h"
#include "Containers/UnrealString.h"

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


void AWeaponObject::SplitWeaponGemData(FString WeaponGemStringData, UStaticMeshComponent* BodyMesh, UStaticMeshComponent* HeadMesh, UStaticMeshComponent* TailMesh)
{
	TArray<FString> ComponentData;
	const TCHAR* PiecesDelim[] =
	{				
		TEXT(";"),
	};
	WeaponGemStringData.ParseIntoArray(ComponentData, PiecesDelim, UE_ARRAY_COUNT(PiecesDelim), true);

	const TCHAR* ComponentDelim[] =
	{				
		TEXT(":"),
	};
	const TCHAR* GemDelim[] =
	{				
		TEXT(","),
	};

	for(FString CData: ComponentData)
	{
		TArray<FString> PieceData; // Example: Body:Gem1,Gem2 -> Body | Gem1,Gem2
		CData.ParseIntoArray(PieceData, ComponentDelim, UE_ARRAY_COUNT(ComponentDelim), true);

		UStaticMeshComponent* TargetMesh = nullptr;
		if(PieceData[0] == "Body")
		{
			TargetMesh = BodyMesh;
		}
		else if(PieceData[0] == "Head")
		{
			TargetMesh = HeadMesh;	
		}
		else if(PieceData[0] == "Tail")
		{
			TargetMesh = TailMesh;
		}

		TArray<FString> GemData; // Example: Gem1,Gem2 -> Gem1 | Gem2
		PieceData[1].ParseIntoArray(GemData, GemDelim, UE_ARRAY_COUNT(GemDelim), true);

		int index = 0;
		for (FString Gem : GemData)
		{
			if(Gem != "Null")
			{
				FString SocketName = "Gem Socket " + index;
				TargetMesh->GetSocketLocation(*SocketName);
				
				UStaticMeshComponent* s = CreateDefaultSubobject<UStaticMeshComponent>(TEXT("Gem" + index));
				s->SetupAttachment(TargetMesh, *SocketName);
				
				if(GEngine)
    				GEngine->AddOnScreenDebugMessage(-1, 15.0f, FColor::Yellow, FString::Printf(TEXT("Looking Socket: %s"), *Gem));	
			}
			
			index++;
		}
		
	}
}

TArray<FString> AWeaponObject::SplitGemData(FString GemData)
{
	return TArray<FString>();
}

