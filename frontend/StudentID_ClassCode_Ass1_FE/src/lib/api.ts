export type Department = { departmentId: number; departmentName: string; departmentDescription: string; isActive: boolean; projects?: Project[] };
export type Project = { projectId: number; projectName: string; description?: string; startDate: string; endDate?: string; status: number; departmentId: number; departmentName: string; isActive: boolean; createdDate?: string; tasks?: WorkTask[] };
export type Tag = { tagId: number; tagName: string; color?: string };
export type WorkTask = { taskId: number; title: string; description?: string; status: number; priority: number; dueDate?: string; projectId: number; projectName: string; createdDate?: string; modifiedDate?: string; tags: Tag[] };
export type DepartmentInput = { departmentName: string; departmentDescription: string };
export type ProjectInput = { projectName: string; description?: string; startDate: string; endDate?: string | null; status: number; departmentId: number };
export type TaskInput = { title: string; description?: string; status: number; priority: number; dueDate?: string | null; projectId: number; tagIds: number[] };
export type TagInput = { tagName: string; color?: string | null };

const API_URL = (process.env.NEXT_PUBLIC_API_URL || "http://localhost:5000").replace(/\/$/, "");

export async function api<T>(path: string, init?: RequestInit): Promise<T> {
  const response = await fetch(`${API_URL}${path}`, { ...init, headers: { "Content-Type": "application/json", ...(init?.headers || {}) } });
  if (!response.ok) {
    const body = await response.json().catch(() => ({}));
    const firstFieldError = body.errors ? Object.values(body.errors).flat().find(Boolean) : undefined;
    throw new Error(body.message || body.title || firstFieldError || "Request failed");
  }
  if (response.status === 204) return undefined as T;
  return response.json();
}

export const getDepartments = () => api<Department[]>("/api/departments");
export const getProjects = () => api<Project[]>("/api/projects");
export const getProject = (id: string) => api<Project>(`/api/projects/${id}`);
export const getDepartment = (id: string) => api<Department>(`/api/departments/${id}`);
export const getTask = (id: string) => api<WorkTask>(`/api/tasks/${id}`);
export const getTags = () => api<Tag[]>("/api/tags");
export const searchTasks = (query: string) => api<WorkTask[]>(`/api/tasks/search${query ? `?${query}` : ""}`);
export const createDepartment = (data: DepartmentInput) => api<Department>("/api/departments", { method: "POST", body: JSON.stringify(data) });
export const updateDepartment = (id: number, data: DepartmentInput) => api<Department>(`/api/departments/${id}`, { method: "PUT", body: JSON.stringify(data) });
export const deleteDepartment = (id: number) => api<void>(`/api/departments/${id}`, { method: "DELETE" });
export const createProject = (data: ProjectInput) => api<Project>("/api/projects", { method: "POST", body: JSON.stringify(data) });
export const updateProject = (id: number, data: ProjectInput) => api<Project>(`/api/projects/${id}`, { method: "PUT", body: JSON.stringify(data) });
export const deleteProject = (id: number) => api<void>(`/api/projects/${id}`, { method: "DELETE" });
export const createTask = (data: TaskInput) => api<WorkTask>("/api/tasks", { method: "POST", body: JSON.stringify(data) });
export const updateTask = (id: number, data: TaskInput) => api<WorkTask>(`/api/tasks/${id}`, { method: "PUT", body: JSON.stringify(data) });
export const deleteTask = (id: number) => api<void>(`/api/tasks/${id}`, { method: "DELETE" });
export const createTag = (data: TagInput) => api<Tag>("/api/tags", { method: "POST", body: JSON.stringify(data) });
export const updateTag = (id: number, data: TagInput) => api<Tag>(`/api/tags/${id}`, { method: "PUT", body: JSON.stringify(data) });
export const deleteTag = (id: number) => api<void>(`/api/tags/${id}`, { method: "DELETE" });

export const statusLabels = ["Not Started", "In Progress", "Completed", "On Hold"];
export const taskStatusLabels = ["To Do", "In Progress", "Done", "Cancelled"];
export const priorityLabels = ["Low", "Medium", "High", "Critical"];
