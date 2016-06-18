cd /d "C:\Projetos\GitHub\EduCon\Fontes\EduCon\EduCon.ImportacaoServico" &msbuild "EduCon.ImportacaoServico.csproj" /t:sdvViewer /p:configuration="Debug" /p:platform=Any CPU
exit %errorlevel% 