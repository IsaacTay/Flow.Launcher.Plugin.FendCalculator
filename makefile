include .env
PLUGIN_NAME ?= FendCalculator

.PHONY: all
all: install

.PHONY: build
build:
	dotnet publish -c Release -r win-x64 --no-self-contained Flow.Launcher.Plugin.FendCalculator.csproj

.PHONY: install
install: build
	@$(if $(PLUGIN_DIR),,$(error PLUGIN_DIR is not defined))
	(taskkill /IM "Flow.Launcher.exe" /F)
	(rm -Recurse -Force "$(PLUGIN_DIR)\$(PLUGIN_NAME)")
	(mv -Force bin\Release\win-x64\publish "$(PLUGIN_DIR)\$(PLUGIN_NAME)")
	(& "$(FLOW_LAUNCHER_EXE)")
