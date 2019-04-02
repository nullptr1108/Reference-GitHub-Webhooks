# This script assumes a complete ground up build. 
$location = "Central US"
$rgName = "rg-webhooks"
$storageAcct = "webhookstorageacct"

# Create a resource group.
az group create --name $rgName --location $location

# Create a storage account
az storage account create --name $storageAcct --resource-group $rgName --location $location --sku Standard_LRS --encryption blob
