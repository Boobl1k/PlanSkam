import axios from "../axios";

const endpoints = {
  registration: (data) => axios.post("/auth/register", data),
  login: (data) => axios.post("/auth/login", data),
  getProfile: () => axios.get("/v1/auth/me"),
  updateProfile: (data) => axios.patch("/v1/auth/me", data),
};

export default endpoints;
