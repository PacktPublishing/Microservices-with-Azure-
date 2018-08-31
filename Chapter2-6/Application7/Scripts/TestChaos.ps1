$ClusterName= "stafftracker.westeurope.cloudapp.azure.com:19000"

$CertThumbprint= "9CF34BB93B4EC357CECBC1E5B602F0CD36FC8C3D"

Connect-ServiceFabricCluster -ConnectionEndpoint $ClusterName -KeepAliveIntervalInSec 10   -X509Credential -ServerCertThumbprint $CertThumbprint  -FindType FindByThumbprint    -FindValue $CertThumbprint -StoreLocation CurrentUser   -StoreName My

$clusterHealthPolicy = New-Object -TypeName System.Fabric.Health.ClusterHealthPolicy
$clusterHealthPolicy.MaxPercentUnhealthyNodes = 10
$clusterHealthPolicy.MaxPercentUnhealthyApplications = 20
$clusterHealthPolicy.ConsiderWarningAsError = $False
$clusterHealthPolicy.ApplicationTypeHealthPolicyMap.Add("CriticalAppType", 33)
$context = @{"k1" = "v1";"k2" = "v2"}
Start-ServiceFabricChaos -TimeToRunMinute 6 -MaxConcurrentFaults 3 -MaxClusterStabilizationTimeoutSec 60 -WaitTimeBetweenIterationsSec 30 -WaitTimeBetweenFaultsSec 5 -EnableMoveReplicaFaults -Context $context -ClusterHealthPolicy $clusterHealthPolicy 

$clusterHealthPolicy = New-Object -TypeName System.Fabric.Health.ClusterHealthPolicy
$clusterHealthPolicy.MaxPercentUnhealthyNodes = 10
$clusterHealthPolicy.MaxPercentUnhealthyApplications = 20
$clusterHealthPolicy.ConsiderWarningAsError = $False
$clusterHealthPolicy.ApplicationTypeHealthPolicyMap.Add("CriticalAppType", 33)
$context = @{"k1" = "v1";"k2" = "v2"}
Start-ServiceFabricChaos -TimeToRunMinute 6 -MaxConcurrentFaults 3 -MaxClusterStabilizationTimeoutSec 60 -WaitTimeBetweenIterationsSec 30 -WaitTimeBetweenFaultsSec 5 -EnableMoveReplicaFaults -Context $context -ClusterHealthPolicy $clusterHealthPolicy 


$Now = Get-Date
$EndTime = $Now.ToUniversalTime()
$StartTime = $EndTime.AddMinutes(-6)
Get-ServiceFabricChaosReport -StartTimeUtc $StartTime -EndTimeUtc $EndTime -Verbose
