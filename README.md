# ServiPuntosUy Mobile App
## **🚀 Running the Project on Android & iOS – .NET MAUI**

### **🔧 Prerequisites**

* [.NET 8 SDK](https://dotnet.microsoft.com/download)

* [Visual Studio 2022 or later](https://visualstudio.microsoft.com/) with **.NET MAUI** workload installed

* A physical or virtual Android/iOS device for testing

* Proper platform setup:

  * [Android SDK and emulator](https://developer.android.com/studio)

  * [Xcode and iOS simulator](https://developer.apple.com/xcode/) (macOS only)

### First setup

Execute the file `./setup-hooks.sh` to setup the git pre-commit hook.
If you can't run the file, give it permission:
```bash
chmod +x .git/hooks/pre-commit
```

### **🧩 Recommended Extension for Debugging**

It’s highly recommended to install the [.NET Meteor extension](https://marketplace.visualstudio.com/items?itemName=nromanov.dotnet-meteor) for a better debugging experience in .NET MAUI:

Visual Studio \> Extensions \> Manage Extensions \> Search: ".NET Meteor" \> Install

### **📄 Required Setup Before Running**
This project requires a Google Maps API key to be injected into the Android build process.
**Insert your Google Maps API key into the `AndroidManifest.xml`**  
 Go to `Platforms/Android/AndroidManifest.xml` and locate the `meta-data` entry for the Google Maps API:

 \<meta-data android:name="com.google.android.geo.API\_KEY"  
           android:value="{GEO\_API\_KEY}" /\>

1.  Replace `{GEO_API_KEY}` with your actual API key from the [Google Cloud Console](https://console.cloud.google.com/).

2. **Add a valid `appsettings.json`**  
    Ensure your project has a properly configured `appsettings.json` file at the root. This file should include necessary configuration data for the app to run correctly (e.g., API URLs, keys, etc.).

### **▶️ How to Run the Project**

#### **🟢 Android**

1. Connect an Android device or start an emulator.

2. Set the target framework to **Android** in Visual Studio.

Click **Run** or use:

 dotnet build \-t:Run \-f net8.0-android

3. 

#### **🍏 iOS (on macOS)**

1. Connect a physical iOS device or start the iOS simulator.

2. Set the target framework to **iOS** in Visual Studio.

Click **Run** or use:

 dotnet build \-t:Run \-f net8.0-ios

3. 

⚠️ iOS development requires macOS and a valid Apple Developer account.

## Build an APK

Using this command you should be able to create an updated APK file from the project
```bash
dotnet publish -f:net9.0-android -c:Release -p:AndroidPackageFormat=apk
```