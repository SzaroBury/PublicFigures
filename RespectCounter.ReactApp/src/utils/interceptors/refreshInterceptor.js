import axios from "axios";

const api = axios.create({ withCredentials: true });

export const setupRefreshInterceptor = (notify, callHandleLogout) => {
  // console.log("Registering refresh interceptor");
  api.interceptors.response.use(
    response => response,
    async error => {
      if (error.response?.status === 401) {
        notify({message: 'Refreshing tokens...'})
        try {
          await api.post("/api/auth/refresh");
          return api(error.config);
        } catch (refreshError) {
          callHandleLogout()
          return Promise.reject(refreshError);
        }
      }
      return Promise.reject(error);
    }
  );
}

export default api;