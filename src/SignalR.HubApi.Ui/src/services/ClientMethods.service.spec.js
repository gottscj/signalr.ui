import { ClientMethodsService } from "./ClientMethods.service";
import { ClientMethod } from "../models/ClientMethod.model";

describe("ClientMethodsService", () => {
  let sut;

  const maxClientMethods = 5;
  const clientMethodTimeoutMs = 100;
  const cleanUpIntervalMs = 100;

  beforeEach(() => {
    sut = new ClientMethodsService(
      clientMethodTimeoutMs,
      maxClientMethods,
      cleanUpIntervalMs
    );
  });

  describe("add", () => {
    it("should add a client method", () => {
      const expectedClientMethod = new ClientMethod(
        "#FFFFFF",
        "my-service",
        "my-title",
        "my-content"
      );

      sut.add("#FFFFFF", "my-service", "my-title", "my-content");

      expect(sut.clientMethods.length).toEqual(1);
      expect(sut.clientMethods[0]).toStrictEqual(expectedClientMethod);
    });

    it("should limit client methods to the max amount", () => {
      for (let i = 0; i < maxClientMethods + 5; ++i) {
        sut.add("#FFFFFF", "my-service", "my-title", "my-content");
      }

      expect(sut.clientMethods.length).toEqual(maxClientMethods);
    });
  });

  describe("clearAll", () => {
    it("should clear all client methods", () => {
      sut.add("#FFFFFF", "my-service", "my-title", "my-content");
      sut.add("#FFFFFF", "my-service", "my-title", "my-content");

      sut.clearAll();

      expect(sut.clientMethods.length).toEqual(0);
    });
  });

  describe("client method expired", () => {
    it("should be removed", done => {
      sut.add("#FFFFFF", "my-service", "my-title", "my-content");
      sut.add("#FFFFFF", "my-service", "my-title", "my-content");

      expect(sut.clientMethods.length).toEqual(2);
      setTimeout(() => {
        // both client methods must have expired
        expect(sut.clientMethods.length).toEqual(0);
        done();
      }, 3 * clientMethodTimeoutMs);
    });
  });
});
