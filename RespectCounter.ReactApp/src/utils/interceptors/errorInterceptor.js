import api from "./refreshInterceptor";

export const setupErrorInterceptor = (notify) => {
  // console.log("Registering error interceptor");
  api.interceptors.response.use(
    response => response,
    error => {
      const suppressGlobalError = error.config?.suppressGlobalError;
      if (!suppressGlobalError) {
        const message =
          error.response?.data?.message ||
          error.message ||
          "Unknown error";
        notify({message, type: 'error'});
      }
      return Promise.reject(error);
    }
  );
};