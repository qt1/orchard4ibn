if "%WindowsSdkDir%" neq "" goto build

FOR %%v in (12 11) do ( 
  echo %%v 
  FOR %%b in ( 
 	 "%ProgramFiles(x86)%\Microsoft Visual Studio %%v.0\VC\vcvarsall.bat"
   	 "%ProgramFiles%\Microsoft Visual Studio %%v.0\VC\vcvarsall.bat" 
         "D:\Program Files (x86)\Microsoft Visual Studio %%v.0\VC\vcvarsall.bat" 
         "E:\Program Files (x86)\Microsoft Visual Studio %%v.0\VC\vcvarsall.bat" 
      ) do (
      echo %%b
      if exist %%b ( 
         call %%b x86
         goto build
      )
   )
)
  
echo "Unable to detect suitable environment. Build may not succeed."

:build

call build %*

pause
