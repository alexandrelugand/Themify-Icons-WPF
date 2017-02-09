param($installPath, $toolsPath, $package)

$DTE.ItemOperations.Navigate("https://github.com/alexandrelugand/Themify-Icons-WPF/?" + $package.Id + "=" + $package.Version)