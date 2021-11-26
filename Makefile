.ONESHELL:
MAKEFLAGS += --silent

clean:
	# dotnet clean
	rm -rf ./bin
	rm -rf ./obj
.PHONY: clean

build_amd_docker:
	cd scripts/build/
	./docker-clean.sh -a amd64
	./release.sh -a amd64
	./docker-build.sh -a amd64
.PHONY: build_amd_docker

build_arm_docker:
	cd scripts/build/
	./docker-clean.sh -a arm
	./release.sh -a arm
	./docker-build.sh -a arm
.PHONY: build_arm_docker
