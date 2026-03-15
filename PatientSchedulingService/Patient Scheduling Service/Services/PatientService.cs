using Patient_Scheduling_Service.DTOs;
using Patient_Scheduling_Service.Interfaces;
using Patient_Scheduling_Service.Model;
using System.Text.RegularExpressions;

namespace Patient_Scheduling_Service.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<Patients> RegisterPatientAsync(PatientRegistrationDto dto)
        {
            if (!Regex.IsMatch(dto.CCCD, @"^\d{12}$"))
                throw new ArgumentException("CCCD phải bao gồm đúng 12 chữ số.");
            if (await _patientRepository.IsCccdExistsAsync(dto.CCCD))
                throw new ArgumentException("CCCD này đã tồn tại trong hệ thống.");

            var newPatient = new Patients
            {
                FullName = dto.FullName,
                IdentityCard = dto.CCCD,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                Phone = dto.PhoneNumber,
                Address = dto.Address,
                UserID = dto.UserID ?? 0 
            };

            return await _patientRepository.CreateAsync(newPatient);
        }

        public async Task<Patients?> IdentifyPatientAsync(string? cccd, string? phone)
        {
            if (string.IsNullOrEmpty(cccd) && string.IsNullOrEmpty(phone))
                throw new ArgumentException("Vui lòng cung cấp CCCD hoặc Số điện thoại để tra cứu.");

            return await _patientRepository.GetByCccdOrPhoneAsync(cccd, phone);
        }

        public async Task<Patients> UpdatePatientProfileAsync(int patientId, PatientRegistrationDto dto)
        {
            var existingPatient = await _patientRepository.GetByIdAsync(patientId);
            if (existingPatient == null)
                throw new KeyNotFoundException("Không tìm thấy bệnh nhân.");
            
            existingPatient.FullName = dto.FullName;
            existingPatient.DateOfBirth = dto.DateOfBirth;
            existingPatient.Gender = dto.Gender;
            existingPatient.Phone = dto.PhoneNumber;
            existingPatient.Address = dto.Address;

            return await _patientRepository.UpdateAsync(existingPatient);
        }
    }
}
