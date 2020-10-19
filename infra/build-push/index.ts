import * as docker from "@pulumi/docker";
import * as gcp from "@pulumi/gcp";
import * as pulumi from "@pulumi/pulumi";

const config = new pulumi.Config();

const imageName = config.require("image-name");

const weatherApiImage = new docker.Image(imageName, {
    imageName: pulumi.interpolate`gcr.io/${gcp.config.project}/${imageName}:latest`,
    build: {
        context: ".",
    },
});

export const digest = weatherApiImage.digest;
