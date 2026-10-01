
export interface SupportRequestResponse {
  id: string;
  requestCode: string;
  fullname: string;
  email: string;
  subject: number;
  priority: number;
  title: string;
  description: string;
  status: number;
  assignedDepartmentId: string | null;
  assignMessage: string | null;
  userMessage: string | null;
  assignedDepartmentName: string;
}
